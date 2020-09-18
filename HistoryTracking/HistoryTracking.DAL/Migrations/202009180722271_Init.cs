namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Comments = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.Guid(nullable: false),
                        UpdatedByUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Offers_Orders",
                c => new
                    {
                        OfferId = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OfferId, t.OrderId })
                .ForeignKey("dbo.Orders", t => t.OfferId, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OfferId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers_Orders", "OrderId", "dbo.Offers");
            DropForeignKey("dbo.Offers_Orders", "OfferId", "dbo.Orders");
            DropIndex("dbo.Offers_Orders", new[] { "OrderId" });
            DropIndex("dbo.Offers_Orders", new[] { "OfferId" });
            DropTable("dbo.Offers_Orders");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Offers");
        }
    }
}
