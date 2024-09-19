using LlenadoDataFake;
using Microsoft.EntityFrameworkCore;

namespace PaginacionTest.dbcontext
{
    public class PruebaDbContext(DbContextOptions<PruebaDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración adicional de la tabla, si fuera necesaria
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes"); // Nombre de la tabla
                entity.HasKey(c => c.ClienteId); // Definimos la clave primaria
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Direccion).HasMaxLength(200);
                entity.Property(c => c.Email).HasMaxLength(100);
                entity.Property(c => c.FechaNacimiento).IsRequired();
                entity.Property(c => c.FechaCreacion).IsRequired();
                entity.Property(c => c.Empresa).HasMaxLength(100);
            });
        }
    }
}
