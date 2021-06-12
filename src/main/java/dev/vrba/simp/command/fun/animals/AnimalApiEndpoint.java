package dev.vrba.simp.command.fun.animals;

import org.jetbrains.annotations.NotNull;

import java.util.Map;
import java.util.Set;

public interface AnimalApiEndpoint {
    /**
     * Name and aliases that the command should be associated with
     */
    @NotNull Set<String> getNames();

    /**
     * The API endpoint url
     */
    @NotNull String getUrl();

    /**
     * Title of the embed containing the image
     */
    @NotNull String getTitle();

    /**
     * HTTP headers that need to be appended such as API keys etc.
     */
     @NotNull Map<String, String> getHeaders();

     // TODO: Maybe make the return type mono?
     @NotNull String extractImageFromResponse(/* TODO: Add response object*/);
}
