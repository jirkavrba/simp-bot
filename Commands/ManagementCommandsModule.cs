using System.Reflection.Metadata.Ecma335;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using SimpBot.Attributes;
using SimpBot.Database;
using SimpBot.Models;

namespace SimpBot.Commands;

public class ManagementCommandsModule : ModuleBase<SocketCommandContext>
{
    private readonly SimpBotDbContextFactory _dbContextFactory;

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
}