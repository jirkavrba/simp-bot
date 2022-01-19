using Discord.Commands;
using SimpBot;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<CommandService>();
        services.AddHostedService<DiscordBotService>();
    })
    .Build();

await host.RunAsync();