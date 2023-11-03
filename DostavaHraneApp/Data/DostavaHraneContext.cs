using DostavaHrane.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace DostavaHrane.Data
{
    public class DostavaHraneContext : DbContext
    {

        public DbSet<Dostavljac> Dostavljac { get; set; }
        public DbSet<Kupac> Kupac { get; set; }

        public DbSet<Proizvod> Proizvod { get; set; }
        public DbSet<Kosarica> Kosarica { get; set; }
        public DostavaHraneContext(DbContextOptions<DostavaHraneContext> opcije)
            : base(opcije)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kosarica>()
                .HasOne(k => k.Dostavljac)
                .WithMany(d => d.KosaricaDostavljaci)
                .HasForeignKey(k => k.DostavljacSifra);

            modelBuilder.Entity<Kosarica>()
                .HasOne(k => k.Proizvod)
                .WithMany(p => p.KosaricaProizvodi)
                .HasForeignKey(k => k.ProizvodSifra);

            modelBuilder.Entity<Kosarica>()
                .HasOne(k => k.Kupac)
                .WithMany(k => k.KosaricaKupci)
                .HasForeignKey(k => k.KupacSifra);



            base.OnModelCreating(modelBuilder);


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



