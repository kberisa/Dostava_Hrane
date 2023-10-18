using DostavaHrane.Models;
using Microsoft.EntityFrameworkCore;

namespace DostavaHrane.Data
{
    public class DostavaHraneContext : DbContext
    {
        public DostavaHraneContext(DbContextOptions<DostavaHraneContext> opcije)
            : base(opcije)
        {
        }
        public DbSet<Dostavljac> Dostavljac { get; set; }
        public DbSet<Kupac> Kupac { get; set; }

        public DbSet<Proizvod> Proizvod { get; set; }
        public DbSet<Kosarica> Kosarica { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kosarica>().HasOne(k => k.Kupac);
            modelBuilder.Entity<Kosarica>().HasOne(d => d.Dostavljac);
            modelBuilder.Entity<Kosarica>().HasOne(p => p.Proizvod);
        }
    }
}



