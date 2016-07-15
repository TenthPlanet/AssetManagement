namespace AssetManagement.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<AssetManagement.Domain.Context.AssetManagementEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AssetManagement.Domain.Context.AssetManagementEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Employees.AddOrUpdate(
                new Employee { employeeNumber = "1", firstName = "Mark", lastName = "James", fullname = "Mark James", IDNumber = "9358467852415", gender = "Male", hireDate = Convert.ToDateTime("7/15/2016"), position = "Administator", officeNumber = "02", telephoneNumber = "0314958795", mobileNumber = "0726527257", emailAddress = "mark@gmail.com", departmentID = 1},
                 new Employee { employeeNumber = "2", firstName = "Ryan", lastName = "Gigs", fullname = "Ryan Gigs", IDNumber = "8656897444125", gender = "Male", hireDate = Convert.ToDateTime("03/01/2015"), position = "General Staff", officeNumber = "03", telephoneNumber = "0315874789", mobileNumber = "0824578895", emailAddress = "ryan@gmail.com", departmentID = 2 },
                  new Employee { employeeNumber = "3", firstName = "Phindile", lastName = "Mfusi", fullname = "Phindile Mfusi", IDNumber = "9625488845123", gender = "Female", hireDate = Convert.ToDateTime("02/06/2010"), position = "Administator", officeNumber = "04", telephoneNumber = "0315584785", mobileNumber = "0765847521", emailAddress = "phindilemfusi@gmail.com", departmentID = 1 },
                  new Employee { employeeNumber = "4", firstName = "Seth", lastName = "Rymond", fullname = "Seth Rymond", IDNumber = "9456825684571", gender = "Male", hireDate = Convert.ToDateTime("11/07/2010"), position = "General Staff", officeNumber = "05", telephoneNumber = "0316658987", mobileNumber = "0765847521", emailAddress = "seth@yahoo.com", departmentID = 3 },
                  new Employee { employeeNumber = "5", firstName = "Nhlaka", lastName = "Mbotho", fullname = "Nhlaka Mbotho", IDNumber = "9406215688086", gender = "Male", hireDate = Convert.ToDateTime("06/07/2010"), position = "Technician", officeNumber = "06", telephoneNumber = "0318988854", mobileNumber = "0814458795", emailAddress = "nhlakambotho@gmail.com", departmentID = 1 },
                  new Employee { employeeNumber = "6", firstName = "Solomzi", lastName = "Jikani", fullname = "Solomzi Jikani", IDNumber = "9456587874412", gender = "Male", hireDate = Convert.ToDateTime("06/07/2010"), position = "Technician", officeNumber = "07", telephoneNumber = "0318598842", mobileNumber = "0826958795", emailAddress = "meggatym@yahoo.com", departmentID = 1 }
                );
        }
    }
}
