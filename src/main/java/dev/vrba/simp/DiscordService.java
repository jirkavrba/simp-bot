package dev.vrba.simp;

import discord4j.core.DiscordClientBuilder;
import discord4j.core.GatewayDiscordClient;
import discord4j.core.object.presence.Activity;
import discord4j.discordjson.json.gateway.StatusUpdate;
import org.jetbrains.annotations.NotNull;

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

        this.client.onDisconnect().block();
    }

    public void start() {
        this.updatePresence();
    }

    private void updatePresence() {
        this.client.updatePresence(
                StatusUpdate.builder()
                        .afk(true)
                        .status("Online")
                        .game(Activity.playing("with cattos"))
                        .build())
                .subscribe();
    }
}
