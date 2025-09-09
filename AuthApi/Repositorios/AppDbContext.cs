using AuthApi.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace AuthApi.Controllers.Repositorios
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<CategoriaJN> CategoriasJN { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Email unico 
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //Relacion uno a muchos entre Usuario y Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId);

            //categoria
            modelBuilder.Entity<CategoriaJN>(entity =>
            {
                entity.ToTable("categoriajn");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Descripcion).IsRequired().HasMaxLength(255);
            });
        }
    }
}
