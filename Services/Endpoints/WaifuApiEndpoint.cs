using System.Net.Http.Json;

namespace SimpBot.Services.Endpoints;

public class WaifuApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[] {"waifu", "cringe" };

    public override string Url => "https://api.waifu.pics/sfw/waifu";

    public override string? Description => "Brought to you by Lajtkek and Deno being fucking weebs";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["url"];

        return image;
    }
}