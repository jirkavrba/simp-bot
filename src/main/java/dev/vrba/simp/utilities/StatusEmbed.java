package dev.vrba.simp.utilities;

import discord4j.core.object.entity.channel.MessageChannel;
import discord4j.rest.util.Color;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

public class StatusEmbed {

    public static Mono<Void> sendError(@NotNull Mono<MessageChannel> publisher, @NotNull String error) {
        return publisher.flatMap(channel -> sendError(channel, error));
    }

    public static Mono<Void> sendError(@NotNull MessageChannel channel, @NotNull String error) {
        return channel.createEmbed(embed ->
                embed.setTitle("Uh oh!")
                        .setDescription(error)
                        .setColor(Color.of(0xED4245)))
            .then();
    }
}
