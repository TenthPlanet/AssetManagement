namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateAddedDisposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archives", "dateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Archives", "dateAdded");
        }
    }
}
