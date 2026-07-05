using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Muzej.Domain.Entities;
using Muzej.Infrastructure.Identity;

namespace Muzej.Infrastructure
{
    public class MuzejContext : IdentityDbContext<Korisnik>
    {
        public MuzejContext()
        {
        }

        public MuzejContext(DbContextOptions<MuzejContext> options) : base(options)
        {
        }

        public DbSet<Slika> Slike { get; set; }
        public DbSet<Skulptura> Skulpture { get; set; }
        public DbSet<Autor> Autori { get; set; }
        public DbSet<Izlozba> Izlozbe { get; set; }
        public DbSet<StavkaIzlozbe> StavkeIzlozbe { get; set; }
        public DbSet<Ulaznica> Ulaznice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MuzejDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UmetnickoDelo>()
                .HasDiscriminator<string>("TipDela")
                .HasValue<Slika>("Slika")
                .HasValue<Skulptura>("Skulptura");

            modelBuilder.Entity<UmetnickoDelo>()
                .HasOne(d => d.Autor)
                .WithMany(a => a.Dela)
                .HasForeignKey(d => d.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StavkaIzlozbe>()
                .HasOne(si => si.UmetnickoDelo)
                .WithMany(ud => ud.StavkeIzlozbe)
                .HasForeignKey(si => si.UmetnickoDeloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StavkaIzlozbe>()
                .HasOne(si => si.Izlozba)
                .WithMany(i => i.StavkeIzlozbe)
                .HasForeignKey(si => si.IzlozbaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ulaznica>()
                .HasOne(u => u.Izlozba)
                .WithMany(i => i.Ulaznice)
                .HasForeignKey(u => u.IzlozbaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Korisnik>()
                .HasMany(k => k.Ulaznice)
                .WithOne()
                .HasForeignKey(u => u.PosetilacId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}