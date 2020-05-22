using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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

            var context = (this as IObjectContextAdapter).ObjectContext;
            //just after properties of object is set and before object returned from context
            context.ObjectMaterialized += OnObjectMaterialized;
            //just after starting to push values to DB but before update DB
            context.SavingChanges += OnSavingChanges;
        }

        //reject if color Red
        private void OnSavingChanges(object sender, EventArgs e)
        {
            //sender is ObjectContext
            var context = sender as ObjectContext;
            if (context == null)
            {
                return;
            }

            foreach (var item in context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Added))
            {
                if ((item.Entity as Inventory) != null)
                {
                    var entity = (Inventory) item.Entity;
                    if (entity.Color == "Red")
                    {
                        item.RejectPropertyChanges(nameof(entity.Color));
                    }
                }
            }
        }

        private void OnObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
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
