using System.ComponentModel.DataAnnotations;

namespace SimpBot.Models;

public class GuildSettings
{
    public int Id { get; set; }
    
    [Required]
    public ulong GuildId { get; init; }

    [Required]
    [MinLength(1)]
    public string? Prefix { get; set; }
}