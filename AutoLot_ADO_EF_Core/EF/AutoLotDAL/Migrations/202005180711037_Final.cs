namespace AutoLotDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Inventory", newName: "Inventories");
            DropForeignKey("dbo.Orders", "CustId", "dbo.Customers");
            DropForeignKey("dbo.Orders", "CarId", "dbo.Inventory");
            RenameColumn(table: "dbo.Orders", name: "CustId", newName: "CustomerId");
            RenameIndex(table: "dbo.Orders", name: "IX_CustId", newName: "IX_CustomerId");
            DropPrimaryKey("dbo.CreditRisks");
            DropPrimaryKey("dbo.Customers");
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.Inventories");
            DropColumn("dbo.CreditRisks", "CustId");
            DropColumn("dbo.Customers", "CustId");
            DropColumn("dbo.Orders", "OrderId");
            DropColumn("dbo.Inventories", "CarId");
            AddColumn("dbo.CreditRisks", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.CreditRisks", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Customers", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Customers", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Orders", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Orders", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Inventories", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Inventories", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddPrimaryKey("dbo.CreditRisks", "Id");
            AddPrimaryKey("dbo.Customers", "Id");
            AddPrimaryKey("dbo.Orders", "Id");
            AddPrimaryKey("dbo.Inventories", "Id");
            CreateIndex("dbo.CreditRisks", new[] { "LastName", "FirstName" }, unique: true, name: "IDX_CreditRisk_Name");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "CarId", "dbo.Inventories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CarId", "dbo.Inventories");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropPrimaryKey("dbo.Inventories");
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.Customers");
            DropPrimaryKey("dbo.CreditRisks");
            DropColumn("dbo.Inventories", "Timestamp");
            DropColumn("dbo.Inventories", "Id");
            DropColumn("dbo.Orders", "Timestamp");
            DropColumn("dbo.Orders", "Id");
            DropColumn("dbo.Customers", "Timestamp");
            DropColumn("dbo.Customers", "Id");
            DropColumn("dbo.CreditRisks", "Timestamp");
            DropColumn("dbo.CreditRisks", "Id");
            AddColumn("dbo.Inventories", "CarId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Orders", "OrderId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Customers", "CustId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.CreditRisks", "CustId", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.CreditRisks", "IDX_CreditRisk_Name");
            AddPrimaryKey("dbo.Inventories", "CarId");
            AddPrimaryKey("dbo.Orders", "OrderId");
            AddPrimaryKey("dbo.Customers", "CustId");
            AddPrimaryKey("dbo.CreditRisks", "CustId");
            RenameIndex(table: "dbo.Orders", name: "IX_CustomerId", newName: "IX_CustId");
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "CustId");
            AddForeignKey("dbo.Orders", "CarId", "dbo.Inventory", "CarId");
            AddForeignKey("dbo.Orders", "CustId", "dbo.Customers", "CustId", cascadeDelete: true);
            RenameTable(name: "dbo.Inventories", newName: "Inventory");
        }
    }
}
