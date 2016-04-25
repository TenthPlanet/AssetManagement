namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateAssigned : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "assigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Keyboards", "assigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Laptops", "assigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Mice", "assigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Monitors", "assigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PCBoxes", "assigndate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Printers", "assigndate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Printers", "assigndate");
            DropColumn("dbo.PCBoxes", "assigndate");
            DropColumn("dbo.Monitors", "assigndate");
            DropColumn("dbo.Mice", "assigndate");
            DropColumn("dbo.Laptops", "assigndate");
            DropColumn("dbo.Keyboards", "assigndate");
            DropColumn("dbo.Assets", "assigndate");
        }
    }
}
