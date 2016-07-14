using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagement.WebUI.ViewModel.Employee
{
    public class EmployeeViewModel
    {
        [Required]
        public string employeeNumber { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string IDNumber { get; set; }
        [Required]
        public string gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime hireDate { get; set; }
        [Required]
        public string position { get; set; }
        [Required]
        public string mobileNumber { get; set; }
        [Required]
        public string officeNumber { get; set; }
        [Required]
        public string telephoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }
        [Required]
        public string departmentName { get; set; }
        public string laptop { get; set; }
        public string assetNumber { get; set; }

        //DISPLAY VIEW MODEL
        public string fullname { get; set; }
    }
}