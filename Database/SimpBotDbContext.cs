using Microsoft.EntityFrameworkCore;
using SimpBot.Models;

namespace SimpBot.Database;

#nullable disable
public class SimpBotDbContext : DbContext
{
    public SimpBotDbContext(DbContextOptions<SimpBotDbContext> options) : base(options)
    {
    }

    public DbSet<GuildSettings> GuildSettings { get; }
}