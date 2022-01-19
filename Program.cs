using SimpBot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<DiscordBotService>(); })
    .Build();

await host.RunAsync();