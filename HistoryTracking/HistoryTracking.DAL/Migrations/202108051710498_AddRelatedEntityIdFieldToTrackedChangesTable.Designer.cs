﻿// <auto-generated />
namespace HistoryTracking.DAL.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class AddRelatedEntityIdFieldToTrackedChangesTable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddRelatedEntityIdFieldToTrackedChangesTable));
        
        string IMigrationMetadata.Id
        {
            get { return "202108051710498_AddRelatedEntityIdFieldToTrackedChangesTable"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}