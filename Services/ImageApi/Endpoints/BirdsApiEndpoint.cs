using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class BirdsApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[] {"bird", "birb", "birds", "birbs"};

    public override string Url => "https://some-random-api.ml/img/birb";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["link"];

        return image;
    }
}