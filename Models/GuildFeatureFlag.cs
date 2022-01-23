namespace SimpBot.Models;

[Flags]
public enum GuildFeatureFlag
{
   EnableImageApi              = 0b001,
   EnableUrbanDictionary       = 0b010,
   EnableNsfwImageApiEndpoints = 0b100 
}