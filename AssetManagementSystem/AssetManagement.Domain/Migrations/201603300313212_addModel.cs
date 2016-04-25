namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Keyboards", "modelName", c => c.String());
            AddColumn("dbo.Mice", "modelName", c => c.String());
            AlterColumn("dbo.Keyboards", "assigndate", c => c.DateTime());
            AlterColumn("dbo.Laptops", "assigndate", c => c.DateTime());
            AlterColumn("dbo.Mice", "assigndate", c => c.DateTime());
            AlterColumn("dbo.Monitors", "assigndate", c => c.DateTime());
            AlterColumn("dbo.PCBoxes", "assigndate", c => c.DateTime());
            AlterColumn("dbo.Printers", "assigndate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Printers", "assigndate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PCBoxes", "assigndate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Monitors", "assigndate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Mice", "assigndate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Laptops", "assigndate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Keyboards", "assigndate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Mice", "modelName");
            DropColumn("dbo.Keyboards", "modelName");
        }
    }
}
