using Discord;
using Discord.Commands;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
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

    private readonly SimpBotDbContextFactory _factory;

    public ImageApiCommandModule(ImageApiService api, StatsTrackingService stats, SimpBotDbContextFactory factory)
    {
        _api = api;
        _stats = stats;
        _factory = factory;
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
            var enabled = await CheckNsfwEndpoint(endpoint);

            if (!enabled)
            {
                await Context.ReplyErrorAsync(
                    "Sorry, this endpoint is NSFW.",
                    "Either the `nsfw` feature is disabled for this guild,\n or you're using this command in a non-NSFW channel"
                );
                return;
            }
            
            var urls = await _api.FetchImageUrls(endpoint, count);
            
            _stats.TrackUsage("command:gib");
            _stats.TrackUsage($"endpoint:{endpoint.Names.First()}", count);

            var embeds = urls.Select(
                    url => new EmbedBuilder()
                        .WithColor(endpoint.Color)
                        .WithImageUrl(url)
                        .WithFooter(endpoint.Description)
                        .Build()
                )
                .ToArray();

            await Context.Message.ReplyAsync(
                allowedMentions: AllowedMentions.None,
                embeds: embeds
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

    private async Task<bool> CheckNsfwEndpoint(ImageApiEndpoint endpoint)
    {
        if (!endpoint.IsNsfw || Context.Guild == null)
        {
            return true;
        }

        // Using nsfw endpoint in a sfw channel
        var channel = Context.Guild.GetTextChannel(Context.Channel.Id);
        if (!channel.IsNsfw)
        {
            return false;
        }

        await using var context = _factory.GetDbContext();

        var settings = await context.GuildSettings.Cacheable().FirstOrDefaultAsync(s => s.GuildId == Context.Guild.Id);
        var features = settings?.EnabledFeatures;

        if (features.HasValue)
        {
            return (features.Value & GuildFeatureFlag.EnableNsfwImageApiEndpoints) == GuildFeatureFlag.EnableNsfwImageApiEndpoints;
        }

        return true;
    }
}