package dev.vrba.simp.command.fun.animals;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.InputStream;
import java.util.Collections;
import java.util.Map;
import java.util.Optional;
import java.util.Set;

public abstract class AnimalApiEndpoint {
    /**
     * Name and aliases that the command should be associated with
     */
    @NotNull
    public abstract Set<String> getNames();

    /**
     * The API endpoint url
     */
    @NotNull
    public abstract String getUrl();

    /**
     * Title of the embed containing the image
     */
    @NotNull
    public abstract String getTitle();

    /**
     * HTTP headers that need to be appended such as API keys etc.
     */
    @NotNull
    public Map<String, String> getHeaders() {
        return Collections.emptyMap();
    }

    @NotNull
    public abstract Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> response);

    @NotNull
    protected Optional<JsonNode> parseJson(@NotNull InputStream stream) {
        try {
            return Optional.ofNullable(new ObjectMapper().readTree(stream));
        }
        catch (Exception exception) {
            return Optional.empty();
        }
    }
}
