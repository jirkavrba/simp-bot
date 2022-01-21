using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using SimpBot.Database;
using SimpBot.Extensions;

namespace SimpBot;

public class DiscordBotService : BackgroundService
{
    private readonly DiscordSocketClient _client;

    private readonly CommandService _commands;

    private readonly IConfiguration _configuration;

    private readonly IServiceProvider _services;

    public DiscordBotService(
        ILogger<DiscordBotService> logger,
        IConfiguration configuration,
        IServiceProvider services,
        CommandService commands
    )
    {
        _configuration = configuration;
        _services = services;
        _commands = commands;

        _client = new DiscordSocketClient(
            new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                LogGatewayIntentWarnings = true
            }
        );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var token = _configuration["Token"] ?? "Please set up the correct token";

        await UpdateDatabaseAsync();

        _client.Ready += UpdateBotPresence;
        _client.MessageReceived += HandleCommandAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await Task.Delay(-1, stoppingToken);
    }

    private async Task UpdateDatabaseAsync()
    {
        var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;

        await using var context = provider.GetRequiredService<SimpBotDbContext>();
        
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
    }

    private async Task UpdateBotPresence()
    {
        await _client.SetGameAsync("with animal APIs");
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        // Do not handle system and bot messages
        if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot) return;

        var offset = 0;

        if (
            userMessage.HasStringPrefix("pls ", ref offset) ||
            userMessage.HasStringPrefix("Pls ", ref offset) ||
            userMessage.HasMentionPrefix(_client.CurrentUser, ref offset)
        )
        {
            var context = new SocketCommandContext(_client, userMessage);
            var result = await _commands.ExecuteAsync(context, offset, _services);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            {
                await context.ReplyError("Sorry, there was an error.", result.ErrorReason);
            }
        }
    }
}