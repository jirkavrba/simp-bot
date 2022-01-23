using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using SimpBot.Database;
using SimpBot.Models;

namespace SimpBot.Attributes;

public class RequireEnabledFeatureFlagAttribute : PreconditionAttribute
{
    private readonly GuildFeatureFlag _flags;

    public RequireEnabledFeatureFlagAttribute(GuildFeatureFlag flags)
    {
        _flags = flags;
    }
    
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        // If invoked from DMs, allow all features regardless of feature flags
        if (context.Guild == null)
        {
            return PreconditionResult.FromSuccess();
        }

        var factory = services.GetRequiredService<SimpBotDbContextFactory>();
        await using var db = factory.GetDbContext();

        var settings = await db.GuildSettings
            .Where(s => s.GuildId == context.Guild.Id)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            return PreconditionResult.FromSuccess();
        }

        return (settings.EnabledFeatures & _flags) == _flags
            ? PreconditionResult.FromSuccess()
            : PreconditionResult.FromError("Missing the required feature flags");
    }
}