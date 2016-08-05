namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropcatergory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContactUs", "catergory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactUs", "catergory", c => c.String());
        }
    }
}
