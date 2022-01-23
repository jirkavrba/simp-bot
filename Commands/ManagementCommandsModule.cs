using System.Reflection.Metadata.Ecma335;
using Discord;
using Discord.Commands;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using SimpBot.Attributes;
using SimpBot.Database;
using SimpBot.Extensions;
using SimpBot.Models;

namespace SimpBot.Commands;

public class ManagementCommandsModule : ModuleBase<SocketCommandContext>
{
    private readonly SimpBotDbContextFactory _dbContextFactory;

    private readonly IDictionary<string, GuildFeatureFlag> _flags = new Dictionary<string, GuildFeatureFlag>
    {
        {"images", GuildFeatureFlag.EnableImageApi},
        {"nsfw", GuildFeatureFlag.EnableNsfwImageApiEndpoints},
        {"urban", GuildFeatureFlag.EnableUrbanDictionary}
    };


    public ManagementCommandsModule(SimpBotDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    [RequireAdminPrivileges]
    [RequireContext(ContextType.Guild)]
    [Command("prefix")]
    public async Task ChangePrefixCommandAsync([Remainder] string? replacement = null)
    {
        await using var context = _dbContextFactory.GetDbContext();

        var guild = Context.Guild.Id;
        var settings = await context.GuildSettings.FirstOrDefaultAsync(g => g.GuildId == guild);
        var prefix = settings?.Prefix ?? "pls";

        // pls prefix
        if (string.IsNullOrWhiteSpace(replacement))
        {
            await Context.Message.ReplyAsync(embed: new EmbedBuilder()
                .WithTitle($"Current prefix is set to `{prefix}`")
                .WithDescription(
                    $"To change the prefix, use `{prefix} prefix something`,\n where `something` is the new prefix.")
                .WithCurrentTimestamp()
                .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
                .Build()
            );
            return;
        }

        if (settings == null)
        {
            context.GuildSettings.Add(
                new GuildSettings()
                {
                    GuildId = guild,
                    Prefix = replacement
                }
            );
        }
        else
        {
            settings.Prefix = replacement;
            context.GuildSettings.Update(settings);
        }

        await context.SaveChangesAsync();
        await Context.Message.ReplyAsync(embed: new EmbedBuilder()
            .WithTitle($"Prefix changed to `{replacement}`")
            .WithCurrentTimestamp()
            .WithFooter(Context.User.Username, Context.User.GetAvatarUrl())
            .Build()
        );

        var bot = Context.Guild.GetUser(Context.Client.CurrentUser.Id);
        var nickname = replacement == "pls" ? "Simp Bot" : $"Simp bot [{replacement}]";

        await bot.ModifyAsync(b => b.Nickname = nickname);
    }

    [RequireAdminPrivileges]
    [RequireContext(ContextType.Guild)]
    [Command("enable")]
    public async Task EnableFeatureFlag(string key)
    {
        if (!_flags.ContainsKey(key))
        {
            var flags = string.Join("\n", _flags.Keys.Select(f => $"• `{f}`"));
            await Context.ReplyErrorAsync("Unknown feature flag", $"Available flags are:\n{flags}");
        }

        await using var context = _dbContextFactory.GetDbContext();

        var flag = _flags[key];
        var guild = Context.Guild.Id;
        var settings = await context.GuildSettings.FirstOrDefaultAsync(g => g.GuildId == guild)
                       ?? new GuildSettings {GuildId = guild};

        settings.EnabledFeatures |= flag;
        
        context.GuildSettings.Update(settings);

        await context.SaveChangesAsync();
        await Context.Message.ReplyAsync(embed: new EmbedBuilder()
            .WithTitle($"Feature flag `{key}` enabled")
            .WithColor(0x57F287)
            .WithAuthor(Context.User.Username, Context.User.GetAvatarUrl())
            .WithCurrentTimestamp()
            .Build()
        );
    }
    
    [RequireAdminPrivileges]
    [RequireContext(ContextType.Guild)]
    [Command("disable")]
    public async Task DisableFeatureFlag(string key)
    {
        if (!_flags.ContainsKey(key))
        {
            var flags = string.Join("\n", _flags.Keys.Select(f => $"• `{f}`"));
            await Context.ReplyErrorAsync("Unknown feature flag", $"Available flags are:\n{flags}");
        }

        await using var context = _dbContextFactory.GetDbContext();

        var flag = _flags[key];
        var guild = Context.Guild.Id;
        var settings = await context.GuildSettings.Cacheable().FirstOrDefaultAsync(g => g.GuildId == guild)
                       ?? new GuildSettings {GuildId = guild};

        settings.EnabledFeatures &= ~flag;
        
        context.GuildSettings.Update(settings);

        await context.SaveChangesAsync();
        await Context.Message.ReplyAsync(embed: new EmbedBuilder()
            .WithTitle($"Feature flag `{key}` disabled")
            .WithColor(0xED4245)
            .WithAuthor(Context.User.Username, Context.User.GetAvatarUrl())
            .WithCurrentTimestamp()
            .Build()
        );
    }
}