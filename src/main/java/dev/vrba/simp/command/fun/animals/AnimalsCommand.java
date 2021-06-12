package dev.vrba.simp.command.fun.animals;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.utilities.StatusEmbed;
import discord4j.core.object.entity.channel.MessageChannel;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class AnimalsCommand implements Command {

    private final Set<AnimalApiEndpoint> endpoints = Collections.emptySet();

    @Override
    public @NotNull String getName() {
        return "gib";
    }

    @Override
    public @NotNull Mono<Void> execute(@NotNull CommandContext context) {
        List<String> arguments = context.getArguments();

        if (arguments.isEmpty()) {
            return StatusEmbed.sendError(
                context.getChannel(),
                "Missing the endpoint parameter"
            );
        }

        if (arguments.size() > 3) {
            return StatusEmbed.sendError(
                context.getChannel(),
                """
                Whoa, that's a lot of parameters.
                            
                Use either `pls gib <count> <endpoint>`
                or `pls gib <endpoint>`
                """
            );

        }

        // pls gib <count> <endpoint>
        if (arguments.size() == 2) {
            try {
                int count = Integer.parseInt(arguments.get(0));

                return this.handle(
                        context.getChannel(),
                        arguments.get(1),
                        count
                );
            }
            catch (NumberFormatException exception) {
                return StatusEmbed.sendError(context.getChannel(), "Bruh, that's not even a valid number smh.");
            }
        }
        // pls gib <endpoint>
        else {
            return this.handle(context.getChannel(), arguments.get(0), 1);
        }
    }

    private Mono<Void> handle(@NotNull Mono<MessageChannel> channel, @NotNull String endpoint, int count) {
        if (count <= 0) {
            return channel.flatMap(it -> StatusEmbed.sendError(it, "No pics for you... \uD83D\uDE3F"));
        }

        if (count > 10) {
            return channel.flatMap(it -> StatusEmbed.sendError(it, "That's just too much... \uD83D\uDE29"));
        }

        return this.endpoints.stream()
            .filter(api -> api.getNames().contains(endpoint))
            .findFirst()
            .map(api -> this.sendImage(channel, api))
            .orElseGet(() -> StatusEmbed.sendError(
                    channel,
                    String.format(
                        """
                        I don't know this endpoint.
                        Choose one of the following:
                        %s
                        """,
                        this.endpoints.stream()
                                .map(AnimalApiEndpoint::getNames)
                                .map(names -> String.join(", ", names))
                                .collect(Collectors.joining(", "))
                    )
            ));
    }

    private Mono<Void> sendImage(@NotNull Mono<MessageChannel> promise, @NotNull AnimalApiEndpoint endpoint) {
        // TODO: Fetch image etc.
        return promise
                .flatMap(channel -> channel.createMessage(endpoint.getUrl()))
                .then();
    }
}
