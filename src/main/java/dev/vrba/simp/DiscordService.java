package dev.vrba.simp;

import dev.vrba.simp.command.CommandsEventHandler;
import dev.vrba.simp.command.CommandsRegistry;
import dev.vrba.simp.command.fun.animals.AnimalsCommand;
import dev.vrba.simp.command.status.GithubLinksCommand;
import dev.vrba.simp.command.status.HelpCommand;
import dev.vrba.simp.command.status.PingCommand;
import dev.vrba.simp.command.status.UptimeCommand;
import discord4j.core.DiscordClientBuilder;
import discord4j.core.GatewayDiscordClient;
import discord4j.core.object.presence.Activity;
import discord4j.discordjson.json.gateway.StatusUpdate;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.util.Set;

public class DiscordService {

    private final GatewayDiscordClient client;

    public DiscordService(@NotNull String token) {
        this.client = DiscordClientBuilder.create(token)
                .build()
                .login()
                .block();

        if (this.client == null) {
            throw new IllegalStateException("Client initialised as null.");
        }
    }

    public void start() {
        this.updatePresence()
            .then(this.registerCommandHandlers())
            .then(this.client.onDisconnect())
            .block();
    }

    private Mono<Void> registerCommandHandlers() {
        String prefix = "pls "; // pls name: [gib] arguments: [10 cattos]
        CommandsRegistry registry = new CommandsRegistry(Set.of(
                new PingCommand(),
                new UptimeCommand(),
                new GithubLinksCommand(),
                new AnimalsCommand()
        ));

        // Help needs access to CommandsRegistry instance
        registry.register(new HelpCommand(registry));

        return new CommandsEventHandler(prefix, registry).register(this.client);
    }

    private Mono<Void> updatePresence() {
        return this.client.updatePresence(
                StatusUpdate.builder()
                        .afk(false)
                        .status("Online")
                        .build()
                        .withGame(Activity.playing("with doggos"))
        );
    }
}
