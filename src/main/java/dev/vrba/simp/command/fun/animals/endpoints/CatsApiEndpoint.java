package dev.vrba.simp.command.fun.animals.endpoints;

import com.fasterxml.jackson.databind.ObjectMapper;
import dev.vrba.simp.command.fun.animals.AnimalApiEndpoint;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.IOException;
import java.io.InputStream;
import java.util.Map;
import java.util.Optional;
import java.util.Set;

public class CatsApiEndpoint extends AnimalApiEndpoint {

    private final String key;

    public CatsApiEndpoint() {
        try {
            this.key = System.getenv("CAT_API_KEY");

            if (this.key == null) {
                throw new NullPointerException();
            }
        } catch (Exception exception) {
            throw new IllegalStateException("Cannot find `CAT_API_KEY` environmental variable.");
        }
    }

    @Override
    public @NotNull Set<String> getNames() {
        return Set.of("cat", "cats", "catto", "cattos", "pussy", "pussies");
    }

    @Override
    public @NotNull String getUrl() {
        return "https://api.thecatapi.com/v1/images/search";
    }

    @Override
    public @NotNull String getTitle() {
        return "Meow";
    }

    @Override
    public @NotNull Map<String, String> getHeaders() {
        return Map.of("x-api-key", key);
    }

    @Override
    public @NotNull Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> response) {
        return this.extractImageFromJson(response, json->json.get(0).get("url").asText());
    }
}
