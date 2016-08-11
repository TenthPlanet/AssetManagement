namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Progresses", "Employee_employeeNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.Progresses", "Employee_employeeNumber");
            AddForeignKey("dbo.Progresses", "Employee_employeeNumber", "dbo.Employees", "employeeNumber");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Progresses", "Employee_employeeNumber", "dbo.Employees");
            DropIndex("dbo.Progresses", new[] { "Employee_employeeNumber" });
            DropColumn("dbo.Progresses", "Employee_employeeNumber");
        }
    }
}
