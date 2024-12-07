
using ApiAnimes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAnimes.Infra.Context
{
    public class SqlDbContext(DbContextOptions<SqlDbContext> options) : DbContext(options)
    {
        public DbSet<Anime> Animes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>()
                .HasIndex(a => a.Name)
                .IsUnique();
        }
    }

    
    
}