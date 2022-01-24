using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class NsfwWaifuApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[] {"nsfwaifu", "nsfwaifus", "hentai"};

    public override string Url => "https://api.waifu.pics/nsfw/waifu";

    public override bool IsNsfw => true;
    
    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["url"];

        return image;
    }
}