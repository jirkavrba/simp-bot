using Discord.Commands;
using SimpBot.Extensions;
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
            await Context.ReplyError("Uh oh!", "Missing the endpoint parameter");
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
            await Context.ReplyError("Uh oh!", "Bruh, that's not even a valid number.");
            return;
        }

        if (count is <= 0 or > 10)
        {
            await Context.ReplyError("Uh oh!", "Bruh, please choose a number between 1 and 10.");
            return;
        }
        
        await CallImageApiAsync(count, filtered[1]);
    }

    private async Task CallImageApiAsync(int count, string endpoint)
    {
        await ReplyAsync($"Handling {count}x {endpoint}");
    }
}