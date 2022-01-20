using Discord;
using Discord.Commands;

namespace SimpBot.Commands;

public class StatusCommandModule : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    [Summary("Check if the bot is running and connected to the Discord gateway")]
    public async Task PingCommandAsync()
    {
        await Context.Message.ReplyAsync(embed: new EmbedBuilder()
            .WithTitle("🏓 Pong!")
            .WithColor(0x5865F2)
            .WithDescription("The bot is up and running.")
            .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
            .WithCurrentTimestamp()
            .Build()
        );
    }

    [Command("github")]
    [Summary("Provides information about the bot Github repository")]
    public async Task GithubCommandAsync()
    {
        await Context.Message.ReplyAsync(
            "https://github.com/jirkavrba/simp-bot",
            components: new ComponentBuilder()
                .WithButton("📜 Source code", url: "https://github.com/jirkavrba/simp-bot", style: ButtonStyle.Link)
                .WithButton("🪲 Report a bug", url: "https://github.com/jirkavrba/simp-bot/issues/new",
                    style: ButtonStyle.Link)
                .Build()
        );
    }
}