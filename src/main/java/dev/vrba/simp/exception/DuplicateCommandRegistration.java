package dev.vrba.simp.exception;

import org.jetbrains.annotations.NotNull;

public class DuplicateCommandRegistration extends RuntimeException {

    public DuplicateCommandRegistration(@NotNull String name) {
        super("Duplicated command registration with name [" + name + "]");
    }
}
