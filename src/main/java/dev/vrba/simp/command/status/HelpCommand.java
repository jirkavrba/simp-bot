package dev.vrba.simp.command.status;

import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.command.CommandsRegistry;
import dev.vrba.simp.command.annotation.CommandDescription;
import dev.vrba.simp.command.annotation.CommandUsage;
import dev.vrba.simp.command.annotation.ExcludeFromHelpListing;
import discord4j.rest.util.Color;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.time.Instant;
import java.util.stream.Collectors;

@ExcludeFromHelpListing
public class HelpCommand implements Command {

    private final CommandsRegistry registry;


    public HelpCommand(@NotNull CommandsRegistry registry) {
        this.registry = registry;
    }

    @Override
    @NotNull
    public String getName() {
        return "help";
    }

    @Override
    @NotNull
    public Mono<Void> execute(@NotNull CommandContext context) {
        // pls help <command-name>
        if (context.getArguments().size() == 1) {
            return this.registry.findCommandByName(context.getArguments().get(0))
                    .map(command -> this.handleCommandHelp(command, context))
                    .orElseGet(() -> this.handleCommandsHelp(context));
        }

        return this.handleCommandsHelp(context);
    }

    @NotNull
    private Mono<Void> handleCommandsHelp(@NotNull CommandContext context) {
        String description = this.registry.getRegisteredCommands()
                .stream()
                .filter(command -> !command.getClass().isAnnotationPresent(ExcludeFromHelpListing.class))
                .map(command -> this.buildCommandSection(command, true))
                .collect(Collectors.joining("\n"));

        return this.createHelpEmbed(
                context,
                "Help",
                description
        );
    }

    @NotNull
    private Mono<Void> handleCommandHelp(@NotNull Command command, @NotNull CommandContext context) {
        return this.createHelpEmbed(
                context,
                command.getName() + " command",
                this.buildCommandSection(command, false)
        );
    }

    @NotNull
    private String buildCommandSection(@NotNull Command command, boolean includeName) {
        StringBuilder section = new StringBuilder();

        if (includeName) {
            section.append("• Command **").append(command.getName()).append("**\n");
        }

        Class<? extends Command> reflection = command.getClass();

        if (reflection.isAnnotationPresent(CommandDescription.class)) {
            section.append("_").append(reflection.getAnnotation(CommandDescription.class).value()).append("_\n");
        }
        else {
            section.append("_No description provided._\n");
        }

        if (reflection.isAnnotationPresent(CommandUsage.class)) {
            section.append("‣ `")
                .append(String.join("`\n‣ `", reflection.getAnnotation(CommandUsage.class).value()))
                .append("`\n");
        }
        else {
            section.append("_No usages provided_\n");
        }

        return section.toString();
    }

    @NotNull
    private Mono<Void> createHelpEmbed(@NotNull CommandContext context, @NotNull String title, @NotNull String description) {
        return context.getChannel()
                .flatMap(channel -> channel.createEmbed(
                        embed -> embed.setTitle(title)
                                .setDescription(description)
                                .setColor(Color.of(0xEB459E))
                                .setTimestamp(Instant.now())))
                .then();
    }
}
