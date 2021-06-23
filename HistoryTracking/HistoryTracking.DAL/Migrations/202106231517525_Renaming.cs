namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaming : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TrackEntityChanges", new[] { "ParentEntityId" });
            AddColumn("dbo.TrackEntityChanges", "ParentId", c => c.Guid());
            CreateIndex("dbo.TrackEntityChanges", "ParentId");
            DropColumn("dbo.TrackEntityChanges", "ParentEntityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrackEntityChanges", "ParentEntityId", c => c.Guid());
            DropIndex("dbo.TrackEntityChanges", new[] { "ParentId" });
            DropColumn("dbo.TrackEntityChanges", "ParentId");
            CreateIndex("dbo.TrackEntityChanges", "ParentEntityId");
        }
    }
}
