using Discord.Commands;

namespace SimpBot.Attributes;

public class RequiresGuildAttribute : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        return Task.FromResult(
            context.Guild != null 
                ? PreconditionResult.FromSuccess() 
                : PreconditionResult.FromError("Sorry, this command can be only used in guilds.")
        );
    }
}