namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrackingEntityChangesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackEntityChanges",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventType = c.String(),
                        EventDateUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EntityTable = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        TrackingPropertiesWasChanged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "CreatedDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Orders", "UpdatedDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Users", "CreatedDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Users", "UpdatedDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SubscriptionProducts", "CreatedDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SubscriptionProducts", "UpdatedDateUtc", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Orders", "CreatedDate");
            DropColumn("dbo.Orders", "UpdatedDate");
            DropColumn("dbo.Users", "CreatedDate");
            DropColumn("dbo.Users", "UpdatedDate");
            DropColumn("dbo.SubscriptionProducts", "CreatedDate");
            DropColumn("dbo.SubscriptionProducts", "UpdatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubscriptionProducts", "UpdatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.SubscriptionProducts", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Users", "UpdatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Orders", "UpdatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.SubscriptionProducts", "UpdatedDateUtc");
            DropColumn("dbo.SubscriptionProducts", "CreatedDateUtc");
            DropColumn("dbo.Users", "UpdatedDateUtc");
            DropColumn("dbo.Users", "CreatedDateUtc");
            DropColumn("dbo.Orders", "UpdatedDateUtc");
            DropColumn("dbo.Orders", "CreatedDateUtc");
            DropTable("dbo.TrackEntityChanges");
        }
    }
}
