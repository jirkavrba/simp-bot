package dev.vrba.simp.command.fun.urban;

import discord4j.common.util.Snowflake;
import lombok.AllArgsConstructor;
import lombok.Getter;
import org.jetbrains.annotations.NotNull;

import java.time.Instant;
import java.util.List;

@Getter
@AllArgsConstructor
public class UrbanDictionaryResults {

    @NotNull
    private final Instant expiration;

    @NotNull
    private final Snowflake owner;

    private final int current;

    @NotNull
    private final List<UrbanDictionaryResult> results;

}
