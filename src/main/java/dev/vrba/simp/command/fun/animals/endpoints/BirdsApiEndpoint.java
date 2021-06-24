package dev.vrba.simp.command.fun.animals.endpoints;

import dev.vrba.simp.command.fun.animals.AnimalApiEndpoint;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.InputStream;
import java.util.Set;

public class BirdsApiEndpoint extends AnimalApiEndpoint {
    @Override
    @NotNull
    public Set<String> getNames() {
        return Set.of("bird", "birb", "birds", "birbs");
    }

    @Override
    @NotNull
    public String getUrl() {
        return "https://some-random-api.ml/img/birb";
    }

    @Override
    @NotNull
    public String getTitle() {
        return "Tweet!";
    }

    @Override
    @NotNull
    public Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> response) {
        return this.extractImageFromJson(response, json -> json.get("link").asText());
    }
}
