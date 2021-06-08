package dev.vrba.simp.command;

import discord4j.core.GatewayDiscordClient;
import discord4j.core.event.domain.message.MessageCreateEvent;
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
        // TODO: Implement this
        return Mono.empty();
    }

    private boolean shouldHandle(@NotNull MessageCreateEvent event) {
        String content = event.getMessage().getContent();

        return event.getMessage()
                .getAuthor()
                .map(author ->
                        // The author is a member (and not a bot)
                        !author.isBot() &&
                        // The message starts with a command prefix
                        content.startsWith(this.prefix)
                    )
                .orElse(false);
    }
}
