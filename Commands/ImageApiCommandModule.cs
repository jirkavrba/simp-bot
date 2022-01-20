using Discord;
using Discord.Commands;
using SimpBot.Exceptions;
using SimpBot.Extensions;
using SimpBot.Services;

namespace SimpBot.Commands;

public class ImageApiCommandModule : ModuleBase<SocketCommandContext>
{
    private readonly ImageApiService _api;

    public ImageApiCommandModule(ImageApiService api)
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
            await CallImageApiAsync(filtered[0], 1);
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

        await CallImageApiAsync(filtered[1], count);
    }

    private async Task CallImageApiAsync(string name, int count)
    {
        try
        {
            var endpoint = _api.FindEndpoint(name);
            var urls = await _api.FetchImageUrls(endpoint, count);

            var embeds = urls.Select(
                    url => new EmbedBuilder()
                        .WithColor(endpoint.Color)
                        .WithImageUrl(url)
                        .WithFooter(endpoint.Description)
                        .Build()
                )
                .ToArray();

            await Context.Message.ReplyAsync(embeds: embeds);
        }
        catch (ImageEndpointNotFoundException)
        {
            var endpoints = string.Join('\n', _api.Endpoints
                .Select(e => e.Names)
                .Select(e => e.Select(n => $"**{n}**"))
                .Select(e => "• " + string.Join(", ", e))
            );

            await Context.ReplyError(
                "I don't know this endpoint!",
                $"Choose one of the following:\n {endpoints}"
            );
        }
    }
}