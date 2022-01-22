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

    private readonly SimpBotDbContextFactory _dbContextFactory;

    private readonly IConfiguration _configuration;

    private readonly IServiceProvider _services;

    public DiscordBotService(
        IConfiguration configuration,
        IServiceProvider services,
        CommandService commands,
        SimpBotDbContextFactory dbContextFactory
    )
    {
        _configuration = configuration;
        _services = services;
        _commands = commands;
        _dbContextFactory = dbContextFactory;

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
        await using var context = _dbContextFactory.GetDbContext();
        await context.Database.EnsureCreatedAsync();
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
        var prefix = await RetrieveConfiguredPrefixAsync(userMessage);

        if (
            userMessage.HasStringPrefix(prefix + " ", ref offset, StringComparison.OrdinalIgnoreCase) ||
            userMessage.HasStringPrefix(prefix, ref offset, StringComparison.OrdinalIgnoreCase) ||
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

    private async Task<string> RetrieveConfiguredPrefixAsync(SocketMessage message)
    {
       const string fallback = "pls";
       
       // If the message was received in DMs
       if (message.Channel is not IGuildChannel channel)
       {
           return fallback;
       }
       
       await using var context = _dbContextFactory.GetDbContext();

       var guild = channel.Guild.Id;
       var settings = await context.GuildSettings.FirstOrDefaultAsync(s => s.GuildId == guild);

       return settings?.Prefix ?? fallback;
    }
}