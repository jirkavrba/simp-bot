namespace SimpBot.Services.ImageApi.Endpoints;

public class DogsApiEndpoint : ImageApiEndpoint
{
    public override IEnumerable<string> Names { get; } = new[]
    {
        "dog", "doggo", "dogs", "doggos", "pupper", "puppers"
    };

    public override string Url => "https://random.dog/woof?filter=mp4,webm";

    public override async Task<string?> ExtractImageUrlAsync(HttpResponseMessage response)
    {
        var file = await response.Content.ReadAsStringAsync();
        return "https://random.dog/" + file;
    }
}