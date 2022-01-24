using System.Net.Http.Json;

namespace SimpBot.Services.ImageApi.Endpoints;

public class FoxApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[] {"fox", "foxxo", "foxes", "foxxos"};

    public override string Url => "https://randomfox.ca/floof/";

    public virtual uint Color => 0xFC7B03;

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var data = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        var image = data?["image"];

        return image;
    }
}