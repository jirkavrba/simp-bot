using Discord;
using Discord.Commands;

namespace SimpBot.Commands;

public class StatusCommandModule : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task PingAsync()
    {
        await ReplyAsync(embed: new EmbedBuilder()
            .WithTitle("🏓 Pong!")
            .WithColor(0x5865F2)
            .WithDescription("The bot is up and running.")
            .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
            .WithCurrentTimestamp()
            .Build()
        );
    }
}