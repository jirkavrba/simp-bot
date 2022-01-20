using SimpBot.Exceptions;
using SimpBot.Services.Endpoints;

namespace SimpBot.Services;

public class ImageApiService
{
    private readonly HttpClient _client = new();

    private readonly IConfiguration _configuration;

    public ImageApiService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<ImageApiEndpoint> Endpoints { get; } = new HashSet<ImageApiEndpoint>
    {
        new FoxApiEndpoint(),
        new BirdsApiEndpoint(),
        new DogsApiEndpoint()
    };

    public ImageApiEndpoint FindEndpoint(string name)
    {
        return Endpoints.FirstOrDefault(e => e.Names.Contains(name)) ?? throw new ImageEndpointNotFoundException();
    }

    public async Task<List<string>> FetchImageUrls(ImageApiEndpoint endpoint, int count)
    {
        var tasks = Enumerable.Range(0, count).Select(_ => FetchEndpoint(endpoint));
        var urls = await Task.WhenAll(tasks);

        return urls.Where(u => u != null).ToList()!;
    }


    private async Task<string?> FetchEndpoint(ImageApiEndpoint endpoint)
    {
        try
        {
            using var message = new HttpRequestMessage(HttpMethod.Get, endpoint.Url);

            foreach (var (name, value) in endpoint.Headers)
            {
                message.Headers.Add(name, value); 
            }

            var response = await _client.SendAsync(message);
            var url = await endpoint.ExtractImageUrlAsync(response);
            
            return url;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}