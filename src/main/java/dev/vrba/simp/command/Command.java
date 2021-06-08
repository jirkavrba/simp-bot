package dev.vrba.simp.command;

import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

public interface Command {

    @NotNull String getName();

    @NotNull Mono<Void> execute();
}
