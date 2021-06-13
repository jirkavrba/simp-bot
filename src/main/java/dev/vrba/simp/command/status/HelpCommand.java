package dev.vrba.simp.command.status;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.command.CommandsRegistry;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

public class HelpCommand implements Command {

    private final CommandsRegistry registry;

    public HelpCommand(@NotNull CommandsRegistry registry) {
        this.registry = registry;
    }

    @Override
    public @NotNull String getName() {
        return "help";
    }

    @Override
    public @NotNull Mono<Void> execute(@NotNull CommandContext context) {
        return Mono.empty();
    }
}
