namespace SimpBot.Services.ImageApi;

public abstract class ImageApiEndpoint
{
    /// <summary>
    ///     Name and aliases associated with this endpoint. (eg. [cat, catto, cattos, pussy, ...])
    /// </summary>
    public abstract IEnumerable<string> Names { get; }

    /// <summary>
    ///     The API url endpoint
    /// </summary>
    public abstract string Url { get; }

    /// <summary>
    ///     Optional description displayed in the embed footer
    /// </summary>
    public virtual string? Description => null;

    /// <summary>
    ///     Whether the endpoint should be only accessible with NSFW feature flag enabled
    /// </summary>
    public virtual bool IsNsfw => false;

    /// <summary>
    ///     Color of the Discord embed
    /// </summary>
    public virtual uint Color => 0x202225;

    /// <summary>
    ///     HTTP headers that should be passed within the request (eg. Api keys)
    /// </summary>
    public virtual IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    /// <summary>
    ///     Extract the image URL from the returned HTTP response (usually JSON)
    /// </summary>
    /// <param name="response">HTTP returned by the client</param>
    /// <returns>URL of the image that should be posted</returns>
    public abstract Task<string?> ExtractImageUrlAsync(HttpResponseMessage response);
}