using System.ComponentModel.DataAnnotations;

namespace SimpBot.Models;

public class GuildSettings
{
    public int Id { get; set; }
    
    [Required]
    public ulong GuildId { get; init; }

    [Required]
    [MinLength(1)]
    public string? Prefix { get; set; } = "pls";

    [Required]
    public GuildFeatureFlag EnabledFeatures { get; set; } = GuildFeatureFlag.EnableImageApi | GuildFeatureFlag.EnableUrbanDictionary;
}