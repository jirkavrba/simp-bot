package dev.vrba.simp.command.fun.animals.endpoints;

import dev.vrba.simp.command.fun.animals.AnimalApiEndpoint;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.InputStream;
import java.util.Set;

public class DucksApiEndpoint extends AnimalApiEndpoint {

    @Override
    @NotNull
    public Set<String> getNames() {
        return Set.of("duck", "ducks", "quack", "quacks");
    }

    @Override
    @NotNull
    public String getUrl() {
        return "https://random-d.uk/api/v1/random?type=png";
    }

    @Override
    @NotNull
    public String getTitle() {
        return "Quack!";
    }

    @Override
    @NotNull
    public Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> response) {
        return this.extractImageFromJson(response, json -> json.get("url").asText());
    }
}
