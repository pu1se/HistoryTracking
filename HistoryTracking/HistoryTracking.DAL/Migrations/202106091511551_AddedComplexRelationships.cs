namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedComplexRelationships : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAddresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        City = c.String(),
                        HouseAddress = c.String(),
                        UserId = c.Guid(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserContacts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        UserId = c.Guid(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.SubscriptionProducts", "ParentId", c => c.Guid());
            CreateIndex("dbo.SubscriptionProducts", "ParentId");
            AddForeignKey("dbo.SubscriptionProducts", "ParentId", "dbo.SubscriptionProducts", "Id");
            DropColumn("dbo.SubscriptionProducts", "VendorSubscriptionId");
            DropColumn("dbo.SubscriptionProducts", "EntitlementId");
            DropColumn("dbo.SubscriptionProducts", "VendorOrderId");
            DropColumn("dbo.SubscriptionProducts", "OriginalOrderItemId");
            DropColumn("dbo.SubscriptionProducts", "AzureTenantId");
            DropColumn("dbo.SubscriptionProducts", "OfferId");
            DropColumn("dbo.SubscriptionProducts", "Name");
            DropColumn("dbo.SubscriptionProducts", "LastUsageMeteringDateUtc");
            DropColumn("dbo.SubscriptionProducts", "SubscriptionState");
            DropColumn("dbo.SubscriptionProducts", "StartDateUtc");
            DropColumn("dbo.SubscriptionProducts", "EndDateUtc");
            DropColumn("dbo.SubscriptionProducts", "Quantity");
            DropColumn("dbo.SubscriptionProducts", "InvoiceContractId");
            DropColumn("dbo.SubscriptionProducts", "VendorContractId");
            DropColumn("dbo.SubscriptionProducts", "AccountId");
            DropColumn("dbo.SubscriptionProducts", "CustomerId");
            DropColumn("dbo.SubscriptionProducts", "BundleId");
            DropColumn("dbo.SubscriptionProducts", "JsonData");
            DropColumn("dbo.SubscriptionProducts", "MinimumCommitmentMonths");
            DropColumn("dbo.SubscriptionProducts", "InitialPurchaseValue");
            DropColumn("dbo.SubscriptionProducts", "AutoRenewEnabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubscriptionProducts", "AutoRenewEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "InitialPurchaseValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SubscriptionProducts", "MinimumCommitmentMonths", c => c.Byte(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "JsonData", c => c.String());
            AddColumn("dbo.SubscriptionProducts", "BundleId", c => c.Guid());
            AddColumn("dbo.SubscriptionProducts", "CustomerId", c => c.Guid(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "AccountId", c => c.Guid());
            AddColumn("dbo.SubscriptionProducts", "VendorContractId", c => c.Guid(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "InvoiceContractId", c => c.Guid(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SubscriptionProducts", "EndDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SubscriptionProducts", "StartDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SubscriptionProducts", "SubscriptionState", c => c.Int(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "LastUsageMeteringDateUtc", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SubscriptionProducts", "Name", c => c.String());
            AddColumn("dbo.SubscriptionProducts", "OfferId", c => c.Guid(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "AzureTenantId", c => c.Guid(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "OriginalOrderItemId", c => c.Guid(nullable: false));
            AddColumn("dbo.SubscriptionProducts", "VendorOrderId", c => c.String());
            AddColumn("dbo.SubscriptionProducts", "EntitlementId", c => c.String());
            AddColumn("dbo.SubscriptionProducts", "VendorSubscriptionId", c => c.String());
            DropForeignKey("dbo.SubscriptionProducts", "ParentId", "dbo.SubscriptionProducts");
            DropForeignKey("dbo.UserContacts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAddresses", "UserId", "dbo.Users");
            DropIndex("dbo.SubscriptionProducts", new[] { "ParentId" });
            DropIndex("dbo.UserContacts", new[] { "UserId" });
            DropIndex("dbo.UserAddresses", new[] { "UserId" });
            DropColumn("dbo.SubscriptionProducts", "ParentId");
            DropTable("dbo.UserContacts");
            DropTable("dbo.UserAddresses");
        }
    }
}
