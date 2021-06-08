package dev.vrba.simp;

import org.jetbrains.annotations.NotNull;

public class SimpBotApplication {

    public static void main(@NotNull String[] args) {
        if (args.length != 1 || args[0].isBlank()) {
            throw new IllegalArgumentException(
                """
                Please supply the discord token as a program argument.
                You can find your bot token on https://discord.com/developers/applications
                
                eg. $ java -jar ./simp.jar DISCORD_TOKEN
                """
            );
        }

        String token = args[0].trim();
        DiscordService service = new DiscordService(token);

        service.start();
    }
}
