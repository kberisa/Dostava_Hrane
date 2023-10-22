using DostavaHrane.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


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


            //modelBuilder.Entity<Kosarica>()
            //        .HasMany(o => o.Kupac)
            //        .WithMany(o => o.Proizvodi)
            //        .WithMany(o => o.Dostavljaci)
            //        .UsingEntity<Dictionary<string, object>>("kosarica",
            //        o => o.HasOne<Dostavljac>().WithMany().HasForeignKey("dostavljac"),
            //        o => o.HasMany<Proizvod>().WithMany().HasForeignKey("proizvod"),
            //        o => o.HasOne<Kupac>().WithMany().HasForeignKey("kupac"),
            //        o => o.ToTable("kosarica")
            //        );
        }
    }
}



