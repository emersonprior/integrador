using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace EmpresaDeViajes.Models
{
    public class EmpresaDeViajesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public EmpresaDeViajesContext() : base("name=EmpresaDeViajesContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Compra>().ToTable("Compras");
            modelBuilder.Entity<Compra>().HasMany(t => t.CompraExcursion).WithMany().Map(m =>
            {
                m.MapLeftKey("CompraId");
                m.MapRightKey("ExcursionId");
                m.ToTable("CompraExcursiones");
            });
            modelBuilder.Entity<Compra>().HasMany(t => t.CompraTransporte).WithMany().Map(m =>
            {
                m.MapLeftKey("CompraId");
                m.MapRightKey("TransporteId");
                m.ToTable("CompraTransportes");
            });
            modelBuilder.Entity<Destino>().ToTable("Destinos");
            modelBuilder.Entity<Estadia>().ToTable("Estadias");
            modelBuilder.Entity<Excursion>().ToTable("Excursiones");
            modelBuilder.Entity<Excursion>().HasMany(t => t.ExcursionEstadias).WithMany().Map(m =>
            {
                m.MapLeftKey("ExcursionId");
                m.MapRightKey("EstadiaId");              
                m.ToTable("ExcursionesEstadias");
            });
            modelBuilder.Entity<Excursion>().HasMany(t => t.ExcursionesTransportes).WithMany().Map(m =>
            {
                m.MapLeftKey("ExcursionId");
                m.MapRightKey("TransportesId");
                m.ToTable("ExcursionesTransportes");
            });
            modelBuilder.Entity<Transporte>().ToTable("Transportes");

        }


        public System.Data.Entity.DbSet<EmpresaDeViajes.Models.Compra> Compras { get; set; }

        public System.Data.Entity.DbSet<EmpresaDeViajes.Models.Destino> Destinos { get; set; }

        public System.Data.Entity.DbSet<EmpresaDeViajes.Models.Estadia> Estadias { get; set; }

        public System.Data.Entity.DbSet<EmpresaDeViajes.Models.Excursion> Excursiones { get; set; }

        public System.Data.Entity.DbSet<EmpresaDeViajes.Models.Transporte> Transportes { get; set; }

        public System.Data.Entity.DbSet<EmpresaDeViajes.Models.Usuario> Usuarios { get; set; }
    }
}
