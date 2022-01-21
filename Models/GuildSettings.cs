using System.ComponentModel.DataAnnotations;

namespace SimpBot.Models;

public class GuildSettings
{
    [Key]
    public uint GuildId { get; }

    [Required]
    [MinLength(1)]
    public string? Prefix => null;
}