using Microsoft.EntityFrameworkCore;

namespace SpotifyStalker.Data
{
    public class SpotifyStalkerDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }

        public DbSet<ArtistQueryLog> ArtistQueryLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Data Source=BEN-LEGION;Initial Catalog=SpotifyStalker;Integrated Security=true");

        /// <summary>
        /// Fluent API configuration has the highest precedence and will override conventions and data annotations.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
