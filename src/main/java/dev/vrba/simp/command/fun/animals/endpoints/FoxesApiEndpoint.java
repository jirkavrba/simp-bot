package dev.vrba.simp.command.fun.animals.endpoints;

import dev.vrba.simp.command.fun.animals.AnimalApiEndpoint;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.InputStream;
import java.util.Set;

public class FoxesApiEndpoint extends AnimalApiEndpoint {

    @Override
    @NotNull
    public Set<String> getNames() {
        return Set.of("fox", "foxxo", "foxes", "foxxos");
    }

    @Override
    @NotNull
    public String getUrl() {
        return "https://randomfox.ca/floof/";
    }

    @Override
    @NotNull
    public String getTitle() {
        return "What does the fox say?";
    }

    @Override
    @NotNull
    public Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> response) {
        return this.extractImageFromJson(response, json -> json.get("image").asText());
    }
}
