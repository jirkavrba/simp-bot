namespace SimpBot.Services.ImageApi.Endpoints;

public class InspirobotApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names => new[] {"inspirobot", "inspiration", "advice"};

    public override string Url => "https://inspirobot.me/api?generate=true";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
    }
}