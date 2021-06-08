package dev.vrba.simp.command;

import dev.vrba.simp.exception.CommandExecutionException;
import discord4j.core.GatewayDiscordClient;
import discord4j.core.event.domain.message.MessageCreateEvent;
import discord4j.core.object.entity.Guild;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

public class CommandsEventHandler {

    private final String prefix;

    private final CommandsRegistry registry;

    public CommandsEventHandler(@NotNull String prefix, @NotNull CommandsRegistry registry) {
        this.prefix = prefix;
        this.registry = registry;
    }

    public Mono<Void> register(@NotNull GatewayDiscordClient client) {
        return client.on(MessageCreateEvent.class)
                .filter(this::shouldHandle)
                .flatMap(this::handleMessage)
                .then();
    }

    private Mono<Void> handleMessage(@NotNull MessageCreateEvent event) {
        String content = event.getMessage().getContent();
        String name = content.split("\s+")[0].replace(this.prefix, "");

        return this.registry.findCommandByName(name)
                .map(command -> this.handleCommand(command, event))
                .orElse(Mono.empty());
    }

    private Mono<Void> handleCommand(@NotNull Command command, @NotNull MessageCreateEvent event) {
        return event.getGuild()
            .map(guild -> createCommandContext(guild, event))
            .map(command::execute)
            // TODO: properly handle the exception (log / send error message / ..)
            .onErrorContinue(CommandExecutionException.class, (exception, object) -> {})
            .then();
    }

    private CommandContext createCommandContext(@NotNull Guild guild, @NotNull MessageCreateEvent event) {
        return new CommandContext();
    }

    private boolean shouldHandle(@NotNull MessageCreateEvent event) {
        String content = event.getMessage().getContent();

        return event.getMember()
                .map(user -> !user.isBot() && content.startsWith(this.prefix))
                .orElse(false);
    }
}
