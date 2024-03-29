using Discord;
using Discord.Commands;
using SimpBot.Attributes;
using SimpBot.Database;
using SimpBot.Exceptions;
using SimpBot.Extensions;
using SimpBot.Models;
using SimpBot.Services;
using SimpBot.Services.ImageApi;

namespace SimpBot.Commands;

public class ImageApiCommandModule : ModuleBase<SocketCommandContext>
{
    private readonly ImageApiService _api;

    private readonly StatsTrackingService _stats;

    public ImageApiCommandModule(ImageApiService api, StatsTrackingService stats)
    {
        _api = api;
        _stats = stats;
    }

    [Command("image")]
    [Alias("images", "gib", "give", "pic", "pics")]
    [Summary("Downloads and posts image from various API endpoints.\nTo list all available endpoints, use `pls gib`")]
    [RequireEnabledFeatureFlag(GuildFeatureFlag.EnableImageApi)]
    public async Task ImageApiCommand([Remainder] string? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(parameters))
        {
            await Context.ReplyErrorAsync("Uh oh!", "Missing the endpoint parameter");
            return;
        }

        // Allows for the usage of `pls gib another cat` etc.
        var extra = new[] {"a", "some", "another", "me", "more", "of"};
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
            await Context.ReplyErrorAsync("Uh oh!", "Bruh, that's not even a valid number.");
            return;
        }

        if (count is <= 0 or > 10)
        {
            await Context.ReplyErrorAsync("Uh oh!", "Bruh, please choose a number between 1 and 10.");
            return;
        }

        await CallImageApiAsync(filtered[1], count);
    }

    private async Task CallImageApiAsync(string name, int count)
    {
        await Context.Channel.TriggerTypingAsync();
        
        try
        {
            var endpoint = _api.FindEndpoint(name);

            var urls = await _api.FetchImageUrls(endpoint, count);
            var embeds = urls.Select(url => new EmbedBuilder().WithImageUrl(url).Build()).ToArray();
                
            _stats.TrackUsage("command:gib");
            _stats.TrackUsage($"endpoint:{endpoint.Names.First()}", count);
            
            await Context.Channel.SendMessageAsync(
                embeds: embeds,
                allowedMentions: AllowedMentions.None,
                messageReference: Context.Message.Reference
            );
        }
        catch (ImageEndpointNotFoundException)
        {
            var endpoints = string.Join('\n', _api.Endpoints
                .Select(e => e.Names)
                .Select(e => e.Select(n => $"**{n}**"))
                .Select(e => "• " + string.Join(", ", e))
            );

            await Context.ReplyErrorAsync(
                "I don't know this endpoint!",
                $"Choose one of the following:\n {endpoints}"
            );
        }
    }
}