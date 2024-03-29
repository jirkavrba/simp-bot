using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class FoxApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[] {"fox", "foxxo", "foxes", "foxxos"};

    public override string Url => "https://randomfox.ca/floof/";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["image"];

        return image;
    }
}