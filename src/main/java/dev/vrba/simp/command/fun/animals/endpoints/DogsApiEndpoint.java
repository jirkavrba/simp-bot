package dev.vrba.simp.command.fun.animals.endpoints;

import dev.vrba.simp.command.fun.animals.AnimalApiEndpoint;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;

import java.io.InputStream;
import java.util.Set;

public class DogsApiEndpoint extends AnimalApiEndpoint {

    @Override
    @NotNull
    public Set<String> getNames() {
        return Set.of("dog", "doggo", "doge", "dogs", "doggos", "doges", "cheems", "cheemsburger", "pupper");
    }

    @Override
    @NotNull
    public String getUrl() {
        return "https://shibe.online/api/shibes?count=1";
    }

    @Override
    @NotNull
    public String getTitle() {
        return "Woof";
    }

    @Override
    @NotNull
    public Mono<String> extractImageFromResponse(@NotNull Mono<InputStream> response) {
        return this.extractImageFromJson(response, json -> json.get(0).asText());
    }
}
