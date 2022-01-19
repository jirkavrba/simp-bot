using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SimpBot;

public class DiscordBotService : BackgroundService
{
    private readonly ILogger<DiscordBotService> _logger;

    private readonly IConfiguration _configuration;

    private readonly DiscordSocketClient _client;

    private readonly CommandService _commands;

    public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration configuration, CommandService commands)
    {
        _logger = logger;
        _configuration = configuration;
        _commands = commands;
        _client = new DiscordSocketClient(
            new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                LogGatewayIntentWarnings = true
            }
        );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var token = _configuration["Token"] ?? "Please set up the correct token";

        _client.Ready += UpdateBotPresence;
        _client.MessageReceived += HandleCommandAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await Task.Delay(-1, stoppingToken);
    }

    private async Task UpdateBotPresence()
    {
        await _client.SetGameAsync("with animal APIs");
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        // Do not handle system and bot messages
        if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot)
        {
            return;
        }

        var offset = 0;

        if (
            userMessage.HasStringPrefix("pls ", ref offset) ||
            userMessage.HasMentionPrefix(_client.CurrentUser, ref offset)
        )
        {
            var context = new SocketCommandContext(_client, userMessage);
            await _commands.ExecuteAsync(context, offset, null);
        }
    }
}