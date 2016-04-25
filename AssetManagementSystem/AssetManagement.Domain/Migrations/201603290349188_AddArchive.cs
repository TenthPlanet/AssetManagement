namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArchive : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archives",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        category = c.String(),
                        dateDisposed = c.DateTime(nullable: false),
                        employeeNumber = c.String(),
                        employeeName = c.String(),
                    })
                .PrimaryKey(t => t.assetNumber);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Archives");
        }
    }
}
