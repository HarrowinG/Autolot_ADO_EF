using System.Data.Entity.Infrastructure.Interception;
using AutoLotDAL.Interception;
using AutoLotDAL.Models;

namespace AutoLotDAL.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AutoLotDbContext : DbContext
    {
        static readonly DatabaseLogger DatabseLogger = new DatabaseLogger("sqllog.txt", true);
        static readonly ConsoleWriterInterceptor ConsoleWriterInterceptor = new ConsoleWriterInterceptor();
        public AutoLotDbContext()
            : base("name=AutoLotConnection")
        {
            DbInterception.Add(ConsoleWriterInterceptor);
            DatabseLogger.StartLogging();
            DbInterception.Add(DatabseLogger);
        }

        public virtual DbSet<CreditRisk> CreditRisks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);
        }

        protected override void Dispose(bool disposing)
        {
            DbInterception.Remove(ConsoleWriterInterceptor);
            DbInterception.Remove(DatabseLogger);
            DatabseLogger.StopLogging();
            base.Dispose(disposing);
        }
    }
}
