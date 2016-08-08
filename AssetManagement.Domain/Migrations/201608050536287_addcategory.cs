namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "category", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUs", "category");
        }
    }
}
