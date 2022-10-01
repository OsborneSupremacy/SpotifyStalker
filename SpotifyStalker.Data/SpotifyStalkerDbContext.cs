using Microsoft.Extensions.Configuration;

namespace SpotifyStalker.Data;

public class SpotifyStalkerDbContext : DbContext
{
    public DbSet<Artist> Artists { get; set; }

    public DbSet<Track> Tracks { get; set; }

    public DbSet<ArtistQueryLog> ArtistQueryLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            // this is only going to be run on a development machine, so we can add user secrets unconditionally
            .AddUserSecrets<SpotifyStalkerDbContext>();

        var config = builder.Build();

        optionsBuilder.UseSqlServer(config.GetConnectionString("SpotifyStalker"));
    }

    /// <summary>
    /// Fluent API configuration has the highest precedence and will override conventions and data annotations.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
