using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class RacoonsApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names => new[] { "racoon", "racoons" };

    public override string Url => "https://some-random-api.ml/animal/raccoon";
    
    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["image"];

        return image;
    }
}