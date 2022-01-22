
using Discord.Commands;

namespace SimpBot.Attributes;

public class RequireAdminPrivilegesAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        // If the command was run in DMs, completely ignore this check
        if (context.Guild == null)
        {
            return PreconditionResult.FromSuccess();
        }
        
        var configuration = services.GetRequiredService<IConfiguration>();
        var owner = configuration.GetValue<ulong>("BotOwnerId");

        // Allow execution of all commands to the bot owner
        if (context.User.Id == owner)
        {
            return PreconditionResult.FromSuccess();
        }

        var member = await context.Guild.GetUserAsync(context.User.Id);

        // Only allow members with the MANAGE_GUILD permissions to invoke this command
        if (member?.GuildPermissions.ManageGuild ?? false)
        {
            return PreconditionResult.FromSuccess();    
        }
        
        return PreconditionResult.FromError("Sorry, you don't have permissions to run this command");
    }
}