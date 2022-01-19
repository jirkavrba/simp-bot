namespace SimpBot.Services;

public abstract class ImageApiEndpoint
{
    /// <summary>
    /// Name and aliases associated with this endpoint. (eg. [cat, catto, cattos, pussy, ...])
    /// </summary>
    public abstract IEnumerable<string> Names { get; init; }

    /// <summary>
    /// The API url endpoint
    /// </summary>
    public abstract string Url { get; init; }

    /// <summary>
    /// Title of the embed that is containing the image
    /// </summary>
    public abstract string Title { get; init; }

    /// <summary>
    /// HTTP headers that should be passed within the request (eg. Api keys)
    /// </summary>
    public abstract IDictionary<string, string> Headers { get; init; }
    
    // TODO: Add methods for extracting image URL from HTTP response
}