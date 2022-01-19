using Discord;
using Discord.Commands;

namespace SimpBot.Extensions;

public static class CommandContextExtensions
{
    public static async Task ReplyError(this SocketCommandContext context, string title, string description)
    {
        await context.Message.ReplyAsync(
            embed: new EmbedBuilder()
                .WithColor(0xED4245)
                .WithTitle(title)
                .WithDescription(description)
                .WithFooter(context.User.Username, context.User.GetAvatarUrl())
                .WithCurrentTimestamp()
                .Build()
        );
    }
}