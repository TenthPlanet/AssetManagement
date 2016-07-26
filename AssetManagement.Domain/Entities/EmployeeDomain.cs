using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AssetManagement.Domain.Entities
{
    public class Employee
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string employeeNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string fullname { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string IDNumber { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string gender { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime hireDate { get; set; }
        public string position { get; set; }
        public string officeNumber { get; set; }
        public string telephoneNumber { get; set; }
        public string mobileNumber { get; set; }
        public string emailAddress { get; set; }
        public virtual ICollection<Printer> Printers { get; set; }
        public virtual ICollection<PCBox> PCBoxes { get; set; }
        public virtual ICollection<Laptop> Laptops { get; set; }
        public virtual ICollection<Keyboard> Keyboards { get; set; }
        public virtual ICollection<Mouse> Mice { get; set; }
        public virtual ICollection<Monitor> Monitors { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public int departmentID { get; set; }
        public virtual Department Departments { get; set; }
    }

    public class Department
    {
        [Key]
        public int departmentID { get; set; }
        public string departmentName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
