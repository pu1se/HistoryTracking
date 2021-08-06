namespace HistoryTracking.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelatedEntityIdFieldToTrackedChangesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackEntityChanges", "RelatedEntityId", c => c.Guid());
            CreateIndex("dbo.TrackEntityChanges", "RelatedEntityId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TrackEntityChanges", new[] { "RelatedEntityId" });
            DropColumn("dbo.TrackEntityChanges", "RelatedEntityId");
        }
    }
}
