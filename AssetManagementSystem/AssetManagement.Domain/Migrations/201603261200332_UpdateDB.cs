namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        catID = c.Int(nullable: false, identity: true),
                        category = c.String(),
                    })
                .PrimaryKey(t => t.catID);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        stockID = c.Int(nullable: false, identity: true),
                        model = c.String(),
                        manufacturer = c.String(),
                        category = c.String(),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.stockID);
            
            DropTable("dbo.Catergories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Catergories",
                c => new
                    {
                        catID = c.Int(nullable: false, identity: true),
                        model = c.String(),
                        manaufacturer = c.String(),
                        catergory = c.String(),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.catID);
            
            DropTable("dbo.Stocks");
            DropTable("dbo.Categories");
        }
    }
}
