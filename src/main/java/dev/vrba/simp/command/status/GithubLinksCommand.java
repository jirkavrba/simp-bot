package dev.vrba.simp.command.status;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.command.annotation.CommandDescription;
import dev.vrba.simp.command.annotation.CommandUsage;
import discord4j.rest.util.Color;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.time.Instant;

@CommandUsage("pls github")
@CommandDescription("Provides a quick shortcut for viewing the repository and reporting issues")
public class GithubLinksCommand implements Command {

    private final static String REPOSITORY_URL = "https://github.com/jirkavrba/simp-bot";

    @Override
    public @NotNull String getName() {
        return "github";
    }

    @Override
    public @NotNull Mono<Void> execute(@NotNull CommandContext context) {
        return context.getChannel()
                .flatMap(channel -> channel.createEmbed(
                        embed -> embed.setTitle("Simp bot on Github")
                                    .setThumbnail("https://github.githubassets.com/images/modules/logos_page/GitHub-Mark.png")
                                    .setColor(Color.of(0x57F287))
                                    .setTimestamp(Instant.now())
                                    .addField("Repository", REPOSITORY_URL, false)
                                    .addField("Report an issue", REPOSITORY_URL + "/issues/new", false)
                                    .addField("Contributors", REPOSITORY_URL + "/graphs/contributors", false)))
                .then();
    }
}
