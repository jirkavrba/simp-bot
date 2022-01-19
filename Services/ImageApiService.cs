namespace SimpBot.Services;

public class ImageApiService
{
    private readonly HttpClient _client = new HttpClient();
    
    private IEnumerable<ImageApiEndpoint> Endpoints { get; } = new List<ImageApiEndpoint>
    {

    };
}