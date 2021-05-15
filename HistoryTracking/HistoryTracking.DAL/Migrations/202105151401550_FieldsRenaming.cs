namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldsRenaming : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackEntityChanges", "EntityId", c => c.String());
            AddColumn("dbo.TrackEntityChanges", "TrackingPropertiesChanges", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrackEntityChanges", "TrackingPropertiesChanges");
            DropColumn("dbo.TrackEntityChanges", "EntityId");
        }
    }
}
