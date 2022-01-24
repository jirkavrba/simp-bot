using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class DucksApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[] {"duck", "ducks", "quack", "quacks"};

    public override string Url => "https://random-d.uk/api/v1/random?type=png";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["url"];

        return image;
    }
}