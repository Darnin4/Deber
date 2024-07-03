using AppLogins.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLogins.Data
{

    public class ProDBContext : DbContext
    {
        public ProDBContext(DbContextOptions<ProDBContext> options) : base(options)
        {
        }

        public DbSet<TipoProducto> TiposProducto { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Users> Usuarios { get; set; }
        public DbSet<FacturaDetalle> FacturasDetalle { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.ToTable("tipo_producto");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Tipo).HasMaxLength(100);
                entity.Property(e => e.Estado).HasMaxLength(20).HasDefaultValue("activo");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("producto");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Iva).HasMaxLength(100);
                entity.Property(e => e.CodigoBarras).HasMaxLength(100);
                entity.Property(e => e.Estado).HasMaxLength(20).HasDefaultValue("activo");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
                entity.HasOne(e => e.TipoProducto)
                      .WithMany()
                      .HasForeignKey(e => e.IdTipo)
                      .HasConstraintName("FK_producto_tipo_producto");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Clave).IsRequired();
                entity.Property(e => e.NombreUsuario).IsRequired();
                entity.Property(e => e.Rol).IsRequired();

                entity.Property(e => e.Estado).HasDefaultValue("activo");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<FacturaDetalle>(entity =>
            {
                entity.ToTable("factura_detalle");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cantidad).HasColumnType("float");
                entity.Property(e => e.Precio).HasColumnType("float");
                entity.Property(e => e.Descuento).HasColumnType("float");
                entity.Property(e => e.Total).HasColumnType("float");
                entity.Property(e => e.Estado).HasMaxLength(20).HasDefaultValue("activo");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
                entity.HasOne(e => e.Factura)
                      .WithMany()
                      .HasForeignKey(e => e.IdFactura)
                      .HasConstraintName("FK_factura_detalle_factura");
                entity.HasOne(e => e.Producto)
                      .WithMany()
                      .HasForeignKey(e => e.IdProducto)
                      .HasConstraintName("FK_factura_detalle_producto");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Estado).HasDefaultValue("activo");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
                entity.Property(e => e.Nombres).HasMaxLength(100);
                entity.Property(e => e.Apellidos).HasMaxLength(100);
                entity.Property(e => e.Cedula).HasMaxLength(100);
                entity.Property(e => e.Direccion).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(100);
            });



            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("factura");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fecha).HasColumnType("date");
                entity.Property(e => e.Total).HasColumnType("float");
                entity.Property(e => e.Iva).HasColumnType("float");
                entity.Property(e => e.SubTotal).HasColumnType("float");
                entity.Property(e => e.Estado).HasMaxLength(20).HasDefaultValue("activo");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.IdCliente)
                      .HasConstraintName("FK_factura_cliente");
            });
        }
    }
}