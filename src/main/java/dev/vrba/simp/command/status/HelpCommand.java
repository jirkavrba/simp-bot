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
        // pls help <command-name>
        if (context.getArguments().size() == 1) {
            return this.registry.findCommandByName(context.getArguments().get(0))
                    .map(command -> this.handleCommandHelp(command, context))
                    .orElseGet(() -> this.handleCommandsHelp(context));
        }

        return this.handleCommandsHelp(context);
    }

    private @NotNull Mono<Void> handleCommandsHelp(@NotNull CommandContext context) {
        return Mono.empty();
    }

    private @NotNull Mono<Void> handleCommandHelp(@NotNull Command command, @NotNull CommandContext context) {
        return Mono.empty();
    }
}
