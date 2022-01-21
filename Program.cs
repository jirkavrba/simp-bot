using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using SimpBot;
using SimpBot.Database;
using SimpBot.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();
        
        services.AddSingleton<CommandService>();
        services.AddSingleton<ImageApiService>();
        services.AddSingleton<StatsTrackingService>();
        services.AddHostedService<DiscordBotService>();
        services.AddDbContext<SimpBotDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });
    })
    .Build();

await host.RunAsync();