namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComWriter : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Progresses", name: "Employee_employeeNumber", newName: "employeeNumber");
            RenameIndex(table: "dbo.Progresses", name: "IX_Employee_employeeNumber", newName: "IX_employeeNumber");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Progresses", name: "IX_employeeNumber", newName: "IX_Employee_employeeNumber");
            RenameColumn(table: "dbo.Progresses", name: "employeeNumber", newName: "Employee_employeeNumber");
        }
    }
}
