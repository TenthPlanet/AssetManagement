namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPending : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Assets", "assigndate");
            DropColumn("dbo.Keyboards", "assigndate");
            DropColumn("dbo.Laptops", "assigndate");
            DropColumn("dbo.Mice", "assigndate");
            DropColumn("dbo.Monitors", "assigndate");
            DropColumn("dbo.PCBoxes", "assigndate");
            DropColumn("dbo.Printers", "assigndate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Printers", "assigndate", c => c.DateTime());
            AddColumn("dbo.PCBoxes", "assigndate", c => c.DateTime());
            AddColumn("dbo.Monitors", "assigndate", c => c.DateTime());
            AddColumn("dbo.Mice", "assigndate", c => c.DateTime());
            AddColumn("dbo.Laptops", "assigndate", c => c.DateTime());
            AddColumn("dbo.Keyboards", "assigndate", c => c.DateTime());
            AddColumn("dbo.Assets", "assigndate", c => c.DateTime(nullable: false));
        }
    }
}
