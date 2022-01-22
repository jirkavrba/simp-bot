using Microsoft.EntityFrameworkCore;
using SimpBot.Models;

namespace SimpBot.Database;

#nullable disable
public class SimpBotDbContext : DbContext
{
    public virtual DbSet<GuildSettings> GuildSettings { get; set; }

    public SimpBotDbContext(DbContextOptions<SimpBotDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<GuildSettings>().HasKey(s => s.Id);
        model.Entity<GuildSettings>().HasIndex(s => s.GuildId);
    }
}