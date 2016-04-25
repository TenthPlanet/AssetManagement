namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttrHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OwnershipHistories",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        category = c.String(),
                        assignDate = c.DateTime(nullable: false),
                        delocateDate = c.DateTime(nullable: false),
                        employeeNumber = c.String(),
                        employeeFullname = c.String(),
                    })
                .PrimaryKey(t => t.assetNumber);
            
            AddColumn("dbo.Assets", "assigndate", c => c.DateTime());
            AddColumn("dbo.Keyboards", "assigndate", c => c.DateTime());
            AddColumn("dbo.Laptops", "assigndate", c => c.DateTime());
            AddColumn("dbo.Mice", "assigndate", c => c.DateTime());
            AddColumn("dbo.Monitors", "assigndate", c => c.DateTime());
            AddColumn("dbo.PCBoxes", "assigndate", c => c.DateTime());
            AddColumn("dbo.Printers", "assigndate", c => c.DateTime());
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
            DropTable("dbo.OwnershipHistories");
        }
    }
}
