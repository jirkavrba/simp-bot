package dev.vrba.simp.command.status;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.command.annotation.CommandDescription;
import dev.vrba.simp.command.annotation.CommandUsage;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.time.Duration;
import java.time.Instant;
import java.util.List;

@CommandUsage("pls uptime")
@CommandDescription("Replies with embed containing uptime of the currently deployed instance.")
public class UptimeCommand implements Command {

    private final Instant launch;

    public UptimeCommand() {
        this.launch = Instant.now();
    }

    @Override
    public @NotNull String getName() {
        return "uptime";
    }

    @Override
    public @NotNull Mono<Void> execute(@NotNull CommandContext context) {
        Duration uptime = Duration.between(launch, Instant.now());
        String formatted = this.formatDuration(uptime);

        return context.getChannel()
                .flatMap(channel -> channel.createEmbed(embed -> embed
                    .setTitle("Uptime")
                    .setDescription(formatted)))
                .then();
    }

    private @NotNull String formatDuration(@NotNull Duration duration) {
        List<String> parts = List.of(
            plural(duration.toDaysPart(), "day"),
            plural(duration.toHoursPart(), "hour"),
            plural(duration.toMinutesPart(), "minute"),
            plural(duration.toSecondsPart(), "second")
        );

        return String.join(", ", parts);
    }

    private @NotNull String plural(long count, @NotNull String unit) {
        return count + " " + unit + (count == 1 ? "" : "s");
    }
}
