namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedParentEntityId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackEntityChanges", "ParentEntityId", c => c.Guid());
            CreateIndex("dbo.TrackEntityChanges", "ParentEntityId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TrackEntityChanges", new[] { "ParentEntityId" });
            DropColumn("dbo.TrackEntityChanges", "ParentEntityId");
        }
    }
}
