package dev.vrba.simp.command;

import dev.vrba.simp.exception.DuplicateCommandRegistration;
import org.jetbrains.annotations.NotNull;

import java.util.*;
import java.util.stream.Collectors;

public class CommandsRegistry {

    private final Map<String, Command> commands;

    public CommandsRegistry(@NotNull Collection<Command> commands) {
        this.commands = new HashMap<>();
        this.registerAll(commands);
    }

    public void register(@NotNull Command command) {
        String name = command.getName();

        if (this.commands.containsKey(name)) {
            throw new DuplicateCommandRegistration(name);
        }

        this.commands.put(name, command);
    }

    public void registerAll(@NotNull Collection<Command> commands) {
        commands.forEach(this::register);
    }

    @NotNull
    public Set<Class<? extends Command>> getRegisteredCommandClasses() {
        return this.commands.values()
                .stream()
                .map(Command::getClass)
                .collect(Collectors.toSet());
    }

    @NotNull
    public Optional<Command> findCommandByName(@NotNull String name) {
        try {
            return Optional.ofNullable(this.commands.get(name));
        }
        catch (NullPointerException exception) {
            return Optional.empty();
        }
    }
}
