using Discord;
using Discord.Commands;
using SimpBot.Services;

namespace SimpBot.Commands;

public class FunCommandsModule : ModuleBase<SocketCommandContext>
{
    private readonly ImageApiService _api;

    public FunCommandsModule(ImageApiService api)
    {
        _api = api;
    }

    [Command("gib")]
    public async Task ImageApiCommand([Remainder] string? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(parameters))
        {
            await Context.Message.ReplyAsync(
                embed: new EmbedBuilder()
                    .WithColor(0xED4245)
                    .WithTitle("Uh oh")
                    .WithDescription("Missing the endpoint parameter")
                    .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build()
            );
            return;
        }

        // Allows for the usage of `pls gib another cat` etc.
        var extra = new[] {"a", "some", "another", "me", "more"};
        var filtered = parameters.Split(" ")
            .Select(p => p.Trim().ToLower())
            .Where(p => !extra.Contains(p))
            .ToArray();

        // pls gib cat
        if (filtered.Length == 1)
        {
            await CallImageApiAsync(1, filtered[0]);
            return;
        }
        
        // pls gib 10 cats
        if (!int.TryParse(filtered[0], out var count))
        {
            await Context.Message.ReplyAsync(
                embed: new EmbedBuilder()
                    .WithColor(0xED4245)
                    .WithTitle("Uh oh")
                    .WithDescription("Bruh that's not even a valid number!")
                    .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build()
            );
            return;
        }

        if (count is <= 0 or > 10)
        {
            await Context.Message.ReplyAsync(
                embed: new EmbedBuilder()
                    .WithColor(0xED4245)
                    .WithTitle("Uh oh")
                    .WithDescription("Bruh please choose a number between 1 and 10.")
                    .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build()
            );
            return;
        }
        
        await CallImageApiAsync(count, filtered[1]);
    }

    private async Task CallImageApiAsync(int count, string endpoint)
    {
        await ReplyAsync($"Handling {count}x {endpoint}");
    }
}