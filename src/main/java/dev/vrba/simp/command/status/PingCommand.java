package dev.vrba.simp.command.status;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.command.annotation.CommandDescription;
import dev.vrba.simp.command.annotation.CommandUsage;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

@CommandUsage("pls ping")
@CommandDescription("Replies with Pong!. Used mostly to check if the bot is up and running.")
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
