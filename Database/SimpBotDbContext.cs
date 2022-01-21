using Microsoft.EntityFrameworkCore;
using SimpBot.Models;

namespace SimpBot.Database;

#nullable disable
public class SimpBotDbContext : DbContext
{
    public DbSet<GuildSettings> GuildSettings { get; }

    public SimpBotDbContext(DbContextOptions<SimpBotDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);
        
        model.Entity<GuildSettings>().HasKey(s => s.GuildId);
    }
}