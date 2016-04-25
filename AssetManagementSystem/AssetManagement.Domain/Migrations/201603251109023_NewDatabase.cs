namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        assetID = c.Int(nullable: false, identity: true),
                        assetNumber = c.String(),
                        serialNumber = c.String(),
                        catergory = c.String(),
                        manufacturer = c.String(),
                        warranty = c.String(),
                        dateadded = c.DateTime(nullable: false),
                        assignstatus = c.Int(nullable: false),
                        employeeNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.assetID)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        employeeNumber = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(),
                        lastName = c.String(),
                        IDNumber = c.String(),
                        gender = c.String(),
                        hireDate = c.DateTime(nullable: false),
                        position = c.String(),
                        mobileNumber = c.String(),
                        emailAddress = c.String(),
                        departmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.employeeNumber)
                .ForeignKey("dbo.Departments", t => t.departmentID, cascadeDelete: true)
                .Index(t => t.departmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        departmentID = c.Int(nullable: false, identity: true),
                        departmentName = c.String(),
                    })
                .PrimaryKey(t => t.departmentID);
            
            CreateTable(
                "dbo.Keyboards",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        serialNumber = c.String(),
                        manufacturer = c.String(),
                        catergory = c.String(),
                        warranty = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        description = c.String(),
                        assignStatus = c.Int(nullable: false),
                        keyboardType = c.String(),
                        employeeNumber = c.String(maxLength: 128),
                        assetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.assetNumber)
                .ForeignKey("dbo.Assets", t => t.assetID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber)
                .Index(t => t.assetID);
            
            CreateTable(
                "dbo.Laptops",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        serialNumber = c.String(),
                        manufacturer = c.String(),
                        catergory = c.String(),
                        warranty = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        description = c.String(),
                        assignStatus = c.Int(nullable: false),
                        modelName = c.String(),
                        screenSize = c.String(),
                        OS = c.String(),
                        RAM = c.String(),
                        HDD = c.String(),
                        employeeNumber = c.String(maxLength: 128),
                        assetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.assetNumber)
                .ForeignKey("dbo.Assets", t => t.assetID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber)
                .Index(t => t.assetID);
            
            CreateTable(
                "dbo.Mice",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        serialNumber = c.String(),
                        manufacturer = c.String(),
                        catergory = c.String(),
                        warranty = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        description = c.String(),
                        assignStatus = c.Int(nullable: false),
                        mouseType = c.String(),
                        employeeNumber = c.String(maxLength: 128),
                        assetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.assetNumber)
                .ForeignKey("dbo.Assets", t => t.assetID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber)
                .Index(t => t.assetID);
            
            CreateTable(
                "dbo.Monitors",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        serialNumber = c.String(),
                        manufacturer = c.String(),
                        catergory = c.String(),
                        warranty = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        description = c.String(),
                        assignStatus = c.Int(nullable: false),
                        modelName = c.String(),
                        displayType = c.String(),
                        displaySize = c.String(),
                        employeeNumber = c.String(maxLength: 128),
                        assetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.assetNumber)
                .ForeignKey("dbo.Assets", t => t.assetID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber)
                .Index(t => t.assetID);
            
            CreateTable(
                "dbo.PCBoxes",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        serialNumber = c.String(),
                        manufacturer = c.String(),
                        catergory = c.String(),
                        warranty = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        description = c.String(),
                        assignStatus = c.Int(nullable: false),
                        modelName = c.String(),
                        boxType = c.String(),
                        OS = c.String(),
                        RAM = c.String(),
                        HDD = c.String(),
                        employeeNumber = c.String(maxLength: 128),
                        assetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.assetNumber)
                .ForeignKey("dbo.Assets", t => t.assetID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber)
                .Index(t => t.assetID);
            
            CreateTable(
                "dbo.Printers",
                c => new
                    {
                        assetNumber = c.String(nullable: false, maxLength: 128),
                        serialNumber = c.String(),
                        manufacturer = c.String(),
                        catergory = c.String(),
                        warranty = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                        description = c.String(),
                        assignStatus = c.Int(nullable: false),
                        modelName = c.String(),
                        printerType = c.String(),
                        interfaceType = c.String(),
                        employeeNumber = c.String(maxLength: 128),
                        assetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.assetNumber)
                .ForeignKey("dbo.Assets", t => t.assetID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.employeeNumber)
                .Index(t => t.employeeNumber)
                .Index(t => t.assetID);
            
            CreateTable(
                "dbo.Catergories",
                c => new
                    {
                        catID = c.Int(nullable: false, identity: true),
                        model = c.String(),
                        manaufacturer = c.String(),
                        catergory = c.String(),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.catID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Printers", "employeeNumber", "dbo.Employees");
            DropForeignKey("dbo.Printers", "assetID", "dbo.Assets");
            DropForeignKey("dbo.PCBoxes", "employeeNumber", "dbo.Employees");
            DropForeignKey("dbo.PCBoxes", "assetID", "dbo.Assets");
            DropForeignKey("dbo.Monitors", "employeeNumber", "dbo.Employees");
            DropForeignKey("dbo.Monitors", "assetID", "dbo.Assets");
            DropForeignKey("dbo.Mice", "employeeNumber", "dbo.Employees");
            DropForeignKey("dbo.Mice", "assetID", "dbo.Assets");
            DropForeignKey("dbo.Laptops", "employeeNumber", "dbo.Employees");
            DropForeignKey("dbo.Laptops", "assetID", "dbo.Assets");
            DropForeignKey("dbo.Keyboards", "employeeNumber", "dbo.Employees");
            DropForeignKey("dbo.Keyboards", "assetID", "dbo.Assets");
            DropForeignKey("dbo.Employees", "departmentID", "dbo.Departments");
            DropForeignKey("dbo.Assets", "employeeNumber", "dbo.Employees");
            DropIndex("dbo.Printers", new[] { "assetID" });
            DropIndex("dbo.Printers", new[] { "employeeNumber" });
            DropIndex("dbo.PCBoxes", new[] { "assetID" });
            DropIndex("dbo.PCBoxes", new[] { "employeeNumber" });
            DropIndex("dbo.Monitors", new[] { "assetID" });
            DropIndex("dbo.Monitors", new[] { "employeeNumber" });
            DropIndex("dbo.Mice", new[] { "assetID" });
            DropIndex("dbo.Mice", new[] { "employeeNumber" });
            DropIndex("dbo.Laptops", new[] { "assetID" });
            DropIndex("dbo.Laptops", new[] { "employeeNumber" });
            DropIndex("dbo.Keyboards", new[] { "assetID" });
            DropIndex("dbo.Keyboards", new[] { "employeeNumber" });
            DropIndex("dbo.Employees", new[] { "departmentID" });
            DropIndex("dbo.Assets", new[] { "employeeNumber" });
            DropTable("dbo.Catergories");
            DropTable("dbo.Printers");
            DropTable("dbo.PCBoxes");
            DropTable("dbo.Monitors");
            DropTable("dbo.Mice");
            DropTable("dbo.Laptops");
            DropTable("dbo.Keyboards");
            DropTable("dbo.Departments");
            DropTable("dbo.Employees");
            DropTable("dbo.Assets");
        }
    }
}
