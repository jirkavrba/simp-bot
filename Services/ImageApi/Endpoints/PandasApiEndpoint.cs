using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class PandasApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names => new[] { "panda", "pandas" };
    
    public override string Url => "https://some-random-api.ml/img/panda";
    
    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["link"];

        return image;
    }
}