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
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
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
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
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
                        Currency = c.Int(nullable: false),
                        SubscriptionOwnerUserId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SubscriptionOwnerUserId)
                .Index(t => t.SubscriptionOwnerUserId);
            
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
            DropForeignKey("dbo.SubscriptionProducts", "SubscriptionOwnerUserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "CustomerUserId", "dbo.Users");
            DropIndex("dbo.SubscriptionProducts_Orders", new[] { "OrderId" });
            DropIndex("dbo.SubscriptionProducts_Orders", new[] { "SubscriptionProductId" });
            DropIndex("dbo.SubscriptionProducts", new[] { "SubscriptionOwnerUserId" });
            DropIndex("dbo.Orders", new[] { "CustomerUserId" });
            DropTable("dbo.SubscriptionProducts_Orders");
            DropTable("dbo.SubscriptionProducts");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
        }
    }
}
