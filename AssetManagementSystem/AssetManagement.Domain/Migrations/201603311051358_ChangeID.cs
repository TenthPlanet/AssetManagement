namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeID : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OwnershipHistories");
            AddColumn("dbo.OwnershipHistories", "historyID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.OwnershipHistories", "assetNumber", c => c.String());
            AddPrimaryKey("dbo.OwnershipHistories", "historyID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OwnershipHistories");
            AlterColumn("dbo.OwnershipHistories", "assetNumber", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.OwnershipHistories", "historyID");
            AddPrimaryKey("dbo.OwnershipHistories", "assetNumber");
        }
    }
}
