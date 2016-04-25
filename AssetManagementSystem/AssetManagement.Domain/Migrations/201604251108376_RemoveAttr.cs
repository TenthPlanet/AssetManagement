namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAttr : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Keyboards", "description");
            DropColumn("dbo.Keyboards", "keyboardType");
            DropColumn("dbo.Laptops", "description");
            DropColumn("dbo.Mice", "description");
            DropColumn("dbo.Mice", "mouseType");
            DropColumn("dbo.Monitors", "description");
            DropColumn("dbo.Monitors", "displayType");
            DropColumn("dbo.PCBoxes", "description");
            DropColumn("dbo.PCBoxes", "boxType");
            DropColumn("dbo.Printers", "description");
            DropColumn("dbo.Printers", "printerType");
            DropColumn("dbo.Printers", "interfaceType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Printers", "interfaceType", c => c.String());
            AddColumn("dbo.Printers", "printerType", c => c.String());
            AddColumn("dbo.Printers", "description", c => c.String());
            AddColumn("dbo.PCBoxes", "boxType", c => c.String());
            AddColumn("dbo.PCBoxes", "description", c => c.String());
            AddColumn("dbo.Monitors", "displayType", c => c.String());
            AddColumn("dbo.Monitors", "description", c => c.String());
            AddColumn("dbo.Mice", "mouseType", c => c.String());
            AddColumn("dbo.Mice", "description", c => c.String());
            AddColumn("dbo.Laptops", "description", c => c.String());
            AddColumn("dbo.Keyboards", "keyboardType", c => c.String());
            AddColumn("dbo.Keyboards", "description", c => c.String());
        }
    }
}
