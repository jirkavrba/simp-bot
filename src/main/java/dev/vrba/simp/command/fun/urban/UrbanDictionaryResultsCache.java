package dev.vrba.simp.command.fun.urban;

import discord4j.common.util.Snowflake;
import lombok.Getter;
import org.jetbrains.annotations.NotNull;

import java.time.Instant;
import java.util.HashMap;
import java.util.Map;

public class UrbanDictionaryResultsCache {

    @Getter
    private final @NotNull Map<Snowflake, UrbanDictionaryResults> entries = new HashMap<>();

    public void removeOldEntries() {
        final var now = Instant.now();

        entries.entrySet()
                .stream()
                .filter(entry -> now.isAfter(entry.getValue().getExpiration()))
                .forEach(entry -> entries.remove(entry.getKey()));
    }

    public void addEntry(@NotNull Snowflake message, @NotNull UrbanDictionaryResults entry) {
        entries.put(message, entry);
    }
}
