namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTelephone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "officeNumber", c => c.String());
            AddColumn("dbo.Employees", "telephoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "telephoneNumber");
            DropColumn("dbo.Employees", "officeNumber");
        }
    }
}
