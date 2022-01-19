using Discord.Commands;
using SimpBot;
using SimpBot.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<CommandService>();
        services.AddSingleton<ImageApiService>();
        services.AddHostedService<DiscordBotService>();
    })
    .Build();

await host.RunAsync();