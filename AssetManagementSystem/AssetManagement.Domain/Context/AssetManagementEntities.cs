using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Context
{
    public class AssetManagementEntities : DbContext
    {
        public AssetManagementEntities()
            : base("AssetManagementEntities") 
        
        { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Keyboard> Keyboards { get; set; }
        public DbSet<PCBox> PCBoxes { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Monitor> Monitors { get; set; }
        public DbSet<Mouse> Mice { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<OwnershipHistory> Ownerships { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

    }
}
