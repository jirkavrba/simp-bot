using Discord.Commands;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using SimpBot;
using SimpBot.Database;
using SimpBot.Services;
using SimpBot.Services.ImageApi;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();
        
        services.AddHostedService<DiscordBotService>();
        services.AddSingleton<CommandService>();
        services.AddSingleton<ImageApiService>();
        services.AddSingleton<StatsTrackingService>();
        services.AddSingleton<SimpBotDbContextFactory>();
        services.AddEFSecondLevelCache(options => options.UseMemoryCacheProvider().DisableLogging());
        services.AddDbContext<SimpBotDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });
    })
    .Build();

await host.RunAsync();