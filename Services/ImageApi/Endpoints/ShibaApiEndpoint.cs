using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class ShibaApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[]
    {
        "shiba", "shibas", "doge", "doges", "cheems", "cheemsburger"
    };

    public override string Url => "https://shibe.online/api/shibes?count=1";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<List<string>>();
        var image = data?[0];

        return image;
    }
}