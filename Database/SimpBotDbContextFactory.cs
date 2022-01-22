namespace SimpBot.Database;

public class SimpBotDbContextFactory
{
    private readonly IServiceProvider _services;

    public SimpBotDbContextFactory(IServiceProvider services)
    {
        _services = services;
    }

    public SimpBotDbContext GetDbContext()
    {
        var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        var context = provider.GetRequiredService<SimpBotDbContext>();

        return context;
    }
}