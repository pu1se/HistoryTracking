namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finilizeTrackEntityChangesTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TrackEntityChanges", new[] { "EntityId" });
            AlterColumn("dbo.TrackEntityChanges", "EntityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TrackEntityChanges", "EntityId");
            DropColumn("dbo.TrackEntityChanges", "EntityBeforeChangeSnapshot");
            DropColumn("dbo.TrackEntityChanges", "PropertiesChangesWay1");
            DropColumn("dbo.TrackEntityChanges", "TimeOfWay1");
            DropColumn("dbo.TrackEntityChanges", "PropertiesChangesWay2");
            DropColumn("dbo.TrackEntityChanges", "TimeOfWay2");
            DropColumn("dbo.TrackEntityChanges", "TimeOfGetOldEntity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrackEntityChanges", "TimeOfGetOldEntity", c => c.Double(nullable: false));
            AddColumn("dbo.TrackEntityChanges", "TimeOfWay2", c => c.Double(nullable: false));
            AddColumn("dbo.TrackEntityChanges", "PropertiesChangesWay2", c => c.String());
            AddColumn("dbo.TrackEntityChanges", "TimeOfWay1", c => c.Double(nullable: false));
            AddColumn("dbo.TrackEntityChanges", "PropertiesChangesWay1", c => c.String());
            AddColumn("dbo.TrackEntityChanges", "EntityBeforeChangeSnapshot", c => c.String());
            DropIndex("dbo.TrackEntityChanges", new[] { "EntityId" });
            AlterColumn("dbo.TrackEntityChanges", "EntityId", c => c.Guid());
            CreateIndex("dbo.TrackEntityChanges", "EntityId");
        }
    }
}
