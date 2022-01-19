using Discord;
using Discord.WebSocket;

namespace SimpBot;

public class DiscordBotService : BackgroundService
{
    private readonly ILogger<DiscordBotService> _logger;

    private readonly IConfiguration _configuration;

    private readonly DiscordSocketClient _client;

    public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await Task.Delay(-1, stoppingToken);
    }

    private async Task UpdateBotPresence()
    {
        await _client.SetGameAsync("with animal APIs");
    }
}