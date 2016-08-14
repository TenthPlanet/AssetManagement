namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Progresses", "employeeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Progresses", "employeeName");
        }
    }
}
