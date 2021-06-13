package dev.vrba.simp.command.fun.urban;

import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class UrbanDictionaryResult {
    private final String word;

    private final String definition;

    private final String example;

    private final String link;

    private final int thumbsUp;

    private final int thumbsDown;
}
