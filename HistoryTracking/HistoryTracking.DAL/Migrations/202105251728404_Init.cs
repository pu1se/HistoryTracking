namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Comments = c.String(),
                        OrderStatus = c.Int(nullable: false),
                        PaymentStatus = c.Int(nullable: false),
                        CustomerUserId = c.Guid(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerUserId)
                .Index(t => t.CustomerUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        UserType = c.Int(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubscriptionProducts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistributorMarkupAsPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ResellerMarkupAsPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrackEntityChanges",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ChangeType = c.String(),
                        ChangeDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EntityTable = c.String(),
                        EntityId = c.Guid(),
                        EntityBeforeChangeSnapshot = c.String(),
                        EntityAfterChangeSnapshot = c.String(),
                        PropertiesChangesWay1 = c.String(),
                        TimeOfWay1 = c.Time(nullable: false, precision: 7),
                        PropertiesChangesWay2 = c.String(),
                        TimeOfWay2 = c.Time(nullable: false, precision: 7),
                        PropertiesChangesWay3 = c.String(),
                        TimeOfWay3 = c.Time(nullable: false, precision: 7),
                        ChangedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ChangedByUserId)
                .Index(t => t.ChangeDateUtc)
                .Index(t => t.EntityId)
                .Index(t => t.ChangedByUserId);
            
            CreateTable(
                "dbo.SubscriptionProducts_Users",
                c => new
                    {
                        SubscriptionProductId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubscriptionProductId, t.UserId })
                .ForeignKey("dbo.Users", t => t.SubscriptionProductId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionProducts", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SubscriptionProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SubscriptionProducts_Orders",
                c => new
                    {
                        SubscriptionProductId = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubscriptionProductId, t.OrderId })
                .ForeignKey("dbo.Orders", t => t.SubscriptionProductId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionProducts", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.SubscriptionProductId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionProducts_Orders", "OrderId", "dbo.SubscriptionProducts");
            DropForeignKey("dbo.SubscriptionProducts_Orders", "SubscriptionProductId", "dbo.Orders");
            DropForeignKey("dbo.TrackEntityChanges", "ChangedByUserId", "dbo.Users");
            DropForeignKey("dbo.SubscriptionProducts_Users", "UserId", "dbo.SubscriptionProducts");
            DropForeignKey("dbo.SubscriptionProducts_Users", "SubscriptionProductId", "dbo.Users");
            DropForeignKey("dbo.Orders", "CustomerUserId", "dbo.Users");
            DropIndex("dbo.SubscriptionProducts_Orders", new[] { "OrderId" });
            DropIndex("dbo.SubscriptionProducts_Orders", new[] { "SubscriptionProductId" });
            DropIndex("dbo.SubscriptionProducts_Users", new[] { "UserId" });
            DropIndex("dbo.SubscriptionProducts_Users", new[] { "SubscriptionProductId" });
            DropIndex("dbo.TrackEntityChanges", new[] { "ChangedByUserId" });
            DropIndex("dbo.TrackEntityChanges", new[] { "EntityId" });
            DropIndex("dbo.TrackEntityChanges", new[] { "ChangeDateUtc" });
            DropIndex("dbo.Orders", new[] { "CustomerUserId" });
            DropTable("dbo.SubscriptionProducts_Orders");
            DropTable("dbo.SubscriptionProducts_Users");
            DropTable("dbo.TrackEntityChanges");
            DropTable("dbo.SubscriptionProducts");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
        }
    }
}
