package dev.vrba.simp.command.fun.animals.endpoints;

import com.fasterxml.jackson.databind.ObjectMapper;
import dev.vrba.simp.command.fun.animals.AnimalApiEndpoint;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.IOException;
import java.io.InputStream;
import java.util.Map;
import java.util.Set;

public class CatApiEndpoint implements AnimalApiEndpoint {

    private final String key;

    public CatApiEndpoint() {
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
    public @NotNull Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> promise) {
        return promise.map(stream -> {
            try {
                return new ObjectMapper()
                        .readTree(stream)
                        .get(0)
                        .get("url")
                        .asText();
            }
            // TODO: Fix this shit
            catch (IOException exception) {
                throw new RuntimeException(exception);
            }
        });
    }
}
