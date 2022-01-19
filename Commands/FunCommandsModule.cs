using Discord.Commands;
using SimpBot.Services;

namespace SimpBot.Commands;

public class FunCommandsModule : ModuleBase<SocketCommandContext>
{
    private readonly ImageApiService _api;

    public FunCommandsModule(ImageApiService api)
    {
        _api = api;
    }

    [Command("gib")]
    public async Task ImageApiCommand([Remainder] string? parameters = null)
    {
        await ReplyAsync("Hello there!");
    }
}