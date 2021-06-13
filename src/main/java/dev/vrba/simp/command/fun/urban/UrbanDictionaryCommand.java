package dev.vrba.simp.command.fun.urban;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import dev.vrba.simp.command.Command;
import dev.vrba.simp.command.CommandContext;
import dev.vrba.simp.command.annotation.CommandDescription;
import dev.vrba.simp.command.annotation.CommandUsage;
import dev.vrba.simp.utilities.StatusEmbed;
import discord4j.core.object.entity.channel.MessageChannel;
import discord4j.core.spec.EmbedCreateSpec;
import discord4j.rest.util.Color;
import org.jetbrains.annotations.NotNull;
import reactor.core.publisher.Mono;
import reactor.netty.http.client.HttpClient;

import java.io.InputStream;
import java.time.Instant;
import java.util.function.Consumer;

@CommandUsage({"pls urban boomer", "pls urban tbh", "pls urban gucci gang"})
@CommandDescription("Searches for the requested term on https://urbandictionary.com")
public class UrbanDictionaryCommand implements Command {

    @Override
    public @NotNull String getName() {
        return "urban";
    }

    @Override
    public @NotNull Mono<Void> execute(@NotNull CommandContext context) {
        String search = String.join("%20", context.getArguments()).trim();

        if (search.isEmpty()) {
            return StatusEmbed.sendError(context.getChannel(), "No search query provided.");
        }

        return context.getChannel()
                .flatMap(channel -> this.searchForDefinition(search, channel))
                .then();
    }

    @NotNull
    private Mono<Void> searchForDefinition(@NotNull String search, @NotNull MessageChannel channel) {
        HttpClient client = HttpClient.create().baseUrl("https://api.urbandictionary.com/");

        Mono<InputStream> response = client.get()
                .uri("/v0/define?term=" + search)
                .responseContent()
                .aggregate()
                .asInputStream();

        return response.flatMap(input -> {
            try {
                JsonNode root = new ObjectMapper().readTree(input);

                // There are no matching results
                if (root.get("list").isEmpty()) {
                    return StatusEmbed.sendError(channel, "No results found.");
                }

                JsonNode result = root.get("list").get(0);

                return channel.createEmbed(this.createResultEmbed(
                    new UrbanDictionaryResult(
                        result.get("word").asText(),
                        result.get("definition").asText().replace("[", "").replace("]", ""),
                        result.get("example").asText().replace("[", "").replace("]", ""),
                        result.get("permalink").asText(),
                        result.get("thumbs_up").asInt(),
                        result.get("thumbs_down").asInt()
                    )
                ));
            }
            catch (Exception exception) {
                return StatusEmbed.sendError(channel, "There was an error.\nMaybe urban dictionary API is down?");
            }
        })
        .then();
    }

    @NotNull
    private Consumer<? super EmbedCreateSpec> createResultEmbed(@NotNull UrbanDictionaryResult result) {
        return embed -> embed
                .setAuthor(result.getWord(), result.getLink(), null)
                .setDescription(
                        result.getDefinition() + "\n\n" +
                        "_" + result.getExample().replaceAll("[_*`]", "") + "_")
                .setFooter("\uD83D\uDC4D " + result.getThumbsUp() + " / \uD83D\uDC4E " + result.getThumbsDown(), null)
                .setTimestamp(Instant.now())
                .setColor(Color.of(0x144FE6));
    }
}
