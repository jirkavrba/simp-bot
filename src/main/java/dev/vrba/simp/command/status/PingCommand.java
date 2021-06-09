package dev.vrba.simp.command.status;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

public class PingCommand implements Command {
    @Override
    public @NotNull String getName() {
        return "ping";
    }

    @Override
    public @NotNull Mono<Void> execute(@NotNull CommandContext context) {
        return context.getChannel()
                .flatMap(channel -> channel.createEmbed(embed -> embed.setTitle("Pong \uD83C\uDFD3")))
                .then();
    }
}
