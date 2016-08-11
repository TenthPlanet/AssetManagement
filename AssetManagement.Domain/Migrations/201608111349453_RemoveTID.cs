namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactUs", "ticketid", "dbo.Tickets");
            DropIndex("dbo.ContactUs", new[] { "ticketid" });
            RenameColumn(table: "dbo.ContactUs", name: "ticketid", newName: "Ticket_ticketid");
            AlterColumn("dbo.ContactUs", "Ticket_ticketid", c => c.Int());
            CreateIndex("dbo.ContactUs", "Ticket_ticketid");
            AddForeignKey("dbo.ContactUs", "Ticket_ticketid", "dbo.Tickets", "ticketid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactUs", "Ticket_ticketid", "dbo.Tickets");
            DropIndex("dbo.ContactUs", new[] { "Ticket_ticketid" });
            AlterColumn("dbo.ContactUs", "Ticket_ticketid", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ContactUs", name: "Ticket_ticketid", newName: "ticketid");
            CreateIndex("dbo.ContactUs", "ticketid");
            AddForeignKey("dbo.ContactUs", "ticketid", "dbo.Tickets", "ticketid", cascadeDelete: true);
        }
    }
}
