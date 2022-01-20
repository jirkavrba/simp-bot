using System.Net.Http.Json;

namespace SimpBot.Services.Endpoints;

public class DogsApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[]
        {"dog", "doggo", "doge", "dogs", "doggos", "doges", "cheems", "cheemsburger", "pupper", "puppers"};

    // TODO: Optimize per-endpoint fetching -> return list of urls
    public override string Url => "https://shibe.online/api/shibes?count=1";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<List<string>>();
        var image = data?[0];

        return image;
    }
}