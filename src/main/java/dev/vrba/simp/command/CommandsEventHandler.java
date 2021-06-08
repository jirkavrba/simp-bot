package dev.vrba.simp.command;

import discord4j.core.GatewayDiscordClient;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

public class CommandsEventHandler {

    private final CommandsRegistry registry;

    public CommandsEventHandler(@NotNull CommandsRegistry registry) {
        this.registry = registry;
    }

    public Mono<Void> register(@NotNull GatewayDiscordClient client) {
        // TODO: Implement this
        return Mono.empty();
    }
}
