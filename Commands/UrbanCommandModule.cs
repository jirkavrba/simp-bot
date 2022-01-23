using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Discord;
using Discord.Commands;
using SimpBot.Attributes;
using SimpBot.Extensions;
using SimpBot.Models;
using SimpBot.Services;

namespace SimpBot.Commands;

public class UrbanCommandModule : ModuleBase<SocketCommandContext>
{
    private readonly HttpClient _client;
    
    private readonly StatsTrackingService _stats;

    public UrbanCommandModule(StatsTrackingService stats)
    {
        _stats = stats;
        _client = new HttpClient();
    }

    [Command("urban")]
    [Summary("Searches for the requested term on https://urbandictionary.com")]
    [RequireEnabledFeatureFlag(GuildFeatureFlag.EnableUrbanDictionary)]
    public async Task UrbanCommandAsync([Remainder] string? term = null)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            await Context.ReplyErrorAsync("No term provided", "Use the command like eg. `pls urban boomer`");
            return;
        }
        
        _stats.TrackUsage("command:urban");

        var query = term.Replace(" ", "%20");
        var url = "https://api.urbandictionary.com/v0/define?term=" + query;
        var response = await _client.GetAsync(url);
        var parsed = await response.Content.ReadFromJsonAsync<UrbanDictionaryApiResponse>();

        if (parsed == null || parsed.Definitions.Count == 0)
        {
            await Context.ReplyErrorAsync("The urban API returned no results", "Try searching for something else");
            return;
        }

        var random = new Random();
        var entry = parsed.Definitions.OrderBy(_ => random.Next()).First();

        var definition = entry.Definition.Replace("[", "").Replace("]", "");
        var example = "_" + entry.Example.Replace("[", "").Replace("]", "") + "_";

        var embed = new EmbedBuilder()
            .WithTitle(entry.Word)
            .WithDescription(definition + "\n\n" + example)
            .WithFooter($"{entry.ThumbsUp} 👍 / {entry.ThumbsDown} 👎")
            .WithTimestamp(entry.WrittenOn)
            .WithColor(0xEFFF00)
            .Build();

        var button = new ButtonBuilder()
            .WithStyle(ButtonStyle.Link)
            .WithUrl(entry.Link)
            .WithLabel(entry.Word + " on urbandictionary.com");

        var component = new ComponentBuilder()
            .WithButton(button)
            .Build();

        await Context.Message.ReplyAsync(embed: embed, components: component);
    }

    private class UrbanDictionaryDefinition
    {
        public UrbanDictionaryDefinition(string definition, string link, string word, string example, int thumbsUp,
            int thumbsDown, DateTime writtenOn)
        {
            Definition = definition;
            Link = link;
            Word = word;
            Example = example;
            ThumbsUp = thumbsUp;
            ThumbsDown = thumbsDown;
            WrittenOn = writtenOn;
        }

        [JsonPropertyName("definition")] public string Definition { get; }
        [JsonPropertyName("permalink")] public string Link { get; }
        [JsonPropertyName("word")] public string Word { get; }
        [JsonPropertyName("example")] public string Example { get; }
        [JsonPropertyName("thumbs_up")] public int ThumbsUp { get; }
        [JsonPropertyName("thumbs_down")] public int ThumbsDown { get; }
        [JsonPropertyName("written_on")] public DateTime WrittenOn { get; }
    }

    private class UrbanDictionaryApiResponse
    {
        public UrbanDictionaryApiResponse(List<UrbanDictionaryDefinition> definitions)
        {
            Definitions = definitions;
        }

        [JsonPropertyName("list")] public List<UrbanDictionaryDefinition> Definitions { get; }
    }
}