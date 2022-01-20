using Discord;
using Discord.Commands;
using SimpBot.Services;

namespace SimpBot.Commands;

public class StatusCommandModule : ModuleBase<SocketCommandContext>
{
    private readonly StatsTrackingService _tracking;

    public StatusCommandModule(StatsTrackingService tracking)
    {
        _tracking = tracking;
    }

    [Command("ping")]
    [Summary("Check if the bot is running and connected to the Discord gateway")]
    public async Task PingCommandAsync()
    {
        _tracking.TrackUsage("command:ping");
        
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
        _tracking.TrackUsage("command:github");
        
        await Context.Message.ReplyAsync(
            "https://github.com/jirkavrba/simp-bot",
            components: new ComponentBuilder()
                .WithButton("📜 Source code", url: "https://github.com/jirkavrba/simp-bot", style: ButtonStyle.Link)
                .WithButton("🪲 Report a bug", url: "https://github.com/jirkavrba/simp-bot/issues/new",
                    style: ButtonStyle.Link)
                .Build()
        );
    }

    [Command("stats")]
    [Summary("Provides stats about the bot usage")]
    public async Task StatsCommand()
    {
        _tracking.TrackUsage("command:stats");

        var longest = _tracking.Stats.Keys.Max(k => k.Length);
        var stats = _tracking.Stats
            .OrderByDescending(pair => pair.Value)
            .Select(pair => $"`{pair.Key.PadLeft(longest)}`: **{pair.Value}**");
        
        var embed = new EmbedBuilder()
            .WithColor(0x57F287)
            .WithTitle("Bot usage")
            .WithDescription(string.Join("\n", stats))
            .WithCurrentTimestamp()
            .WithFooter("Usage is tracked since the last restart")
            .Build();

        await Context.Message.ReplyAsync(embed: embed);
    }
}