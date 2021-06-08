package dev.vrba.simp;

import discord4j.core.DiscordClientBuilder;
import discord4j.core.GatewayDiscordClient;
import discord4j.core.object.presence.Activity;
import discord4j.discordjson.json.gateway.StatusUpdate;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

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
                .then(this.client.onDisconnect())
                .block();
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
