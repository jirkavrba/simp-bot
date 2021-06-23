package dev.vrba.simp.command;

import discord4j.core.GatewayDiscordClient;
import discord4j.core.event.domain.message.MessageCreateEvent;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.util.Arrays;
import java.util.List;
import java.util.Set;
import java.util.logging.Logger;
import java.util.stream.Collectors;

public class CommandsEventHandler {

    private final Set<String> prefixes;

    private final CommandsRegistry registry;

    private final Logger logger;

    public CommandsEventHandler(@NotNull Set<String> prefixes, @NotNull CommandsRegistry registry) {
        this.prefixes = prefixes;
        this.registry = registry;
        this.logger = Logger.getLogger(this.getClass().getName());
    }

    public Mono<Void> register(@NotNull GatewayDiscordClient client) {
        return client.on(MessageCreateEvent.class)
                .filter(this::shouldHandle)
                .flatMap(this::handleMessage)
                .onErrorResume(this::handleError)
                .then();
    }

    private Mono<Void> handleError(@NotNull Throwable throwable) {
        this.logger.severe(throwable.getMessage());
        return Mono.empty();
    }

    @NotNull
    private Mono<Void> handleMessage(@NotNull MessageCreateEvent event) {
        String content = this.replaceFirstMatchedPrefix(event.getMessage().getContent());
        String name = content.split("\s+")[0];

        return this.registry.findCommandByName(name)
                .map(command -> this.handleCommand(command, event))
                .orElse(Mono.empty());
    }

    @NotNull
    private String replaceFirstMatchedPrefix(@NotNull String source) {
        String content = source.toLowerCase();
        String prefix = this.prefixes.stream()
                .filter(content::contains)
                .findFirst()
                .orElse("");

        return content.replaceFirst(prefix, "");
    }

    @NotNull
    private Mono<Void> handleCommand(@NotNull Command command, @NotNull MessageCreateEvent event) {
        return command
                .execute(createCommandContext(event))
                .onErrorResume(this::handleError);
    }

    @NotNull
    private CommandContext createCommandContext(@NotNull MessageCreateEvent event) {
        String content = this.replaceFirstMatchedPrefix(event.getMessage().getContent());
        List<String> arguments = Arrays.stream(content.split("\s+")).skip(1).collect(Collectors.toList());

        // Member is already verified in {@link shouldHandle}
        //noinspection OptionalGetWithoutIsPresent
        return new CommandContext(
            event,
            event.getMember().get(),
            event.getGuild(),
            event.getMessage().getChannel(),
            event.getMessage().getUserMentionIds(),
            event.getMessage().getRoleMentionIds(),
            arguments
        );
    }

    private boolean shouldHandle(@NotNull MessageCreateEvent event) {
        String content = event.getMessage().getContent();

        return event.getMember()
                .map(user -> !user.isBot())
                .map(predicate -> predicate && this.prefixes.stream().anyMatch(
                    prefix -> content.toLowerCase().startsWith(prefix)
                ))
                .orElse(false);
    }
}
