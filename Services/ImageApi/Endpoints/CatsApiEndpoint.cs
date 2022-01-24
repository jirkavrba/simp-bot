using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class CatsApiEndpoint : ImageApiEndpoint
{
    public CatsApiEndpoint(string key)
    {
        Headers = new Dictionary<string, string>
        {
            {"x-api-key", key}
        };
    }

    public override IEnumerable<string> Names { get; } = new[]
    {
        "cat", "cats", "catto", "cattos", "pussy", "pussies", "kitty", "kitties", "kitten"
    };

    public override string Url => "https://api.thecatapi.com/v1/images/search";

    public override IDictionary<string, string> Headers { get; }

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IList<IDictionary<string, object>>>();
        var image = data?[0]["url"].ToString();

        return image;
    }
}