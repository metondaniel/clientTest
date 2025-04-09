using Microsoft.EntityFrameworkCore;
using Product.Service.Entities;
using Product.Service.Events;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Product.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Event> EventStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.NomeRazaoSocial).IsRequired().HasMaxLength(150);
                entity.HasIndex(c => c.CpfCnpj).IsUnique();
                entity.HasIndex(c => c.Email).IsUnique();

                entity.OwnsOne(c => c.Endereco, endereco =>
                {
                    endereco.Property(e => e.Cep).HasColumnName("Cep").HasMaxLength(8);
                    endereco.Property(e => e.Logradouro).HasMaxLength(200);
                    endereco.Property(e => e.Numero).HasMaxLength(10);
                    endereco.Property(e => e.Bairro).HasMaxLength(100);
                    endereco.Property(e => e.Cidade).HasMaxLength(100);
                    endereco.Property(e => e.Estado).HasMaxLength(2);
                });
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EventType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DataOcorrencia).IsRequired();
            });
        }
    }
}
