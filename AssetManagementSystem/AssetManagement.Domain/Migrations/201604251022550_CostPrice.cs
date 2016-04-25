namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "costprice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "costprice");
        }
    }
}
