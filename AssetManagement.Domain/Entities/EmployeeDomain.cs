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
        public string employeeNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullname { get; set; }
        public string IDNumber { get; set; }
        public string gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime hireDate { get; set; }
        //position used in place of role
        //Roles.RoleName
        public string position { get; set; }
        public string officeNumber { get; set; }
        public string telephoneNumber { get; set; }
        public string mobileNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }

        //Profile picture
        public string fileName { get; set; }
        public byte[] fileBytes { get; set; }
        public string fileType { get; set; }

        public virtual ICollection<Printer> Printers { get; set; }
        public virtual ICollection<PCBox> PCBoxes { get; set; }
        public virtual ICollection<Laptop> Laptops { get; set; }
        public virtual ICollection<Keyboard> Keyboards { get; set; }
        public virtual ICollection<Mouse> Mice { get; set; }
        public virtual ICollection<Monitor> Monitors { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public ICollection<Progress> Comments { get; set; }
        //Departments
        public int departmentID { get; set; }
        public virtual Department Departments { get; set; }

        //Roles/Position
        public int RoleID { get; set; }
        public virtual Role Roles { get; set; }
    }

    public class Department
    {
        [Key]
        public int departmentID { get; set; }
        public string departmentName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
