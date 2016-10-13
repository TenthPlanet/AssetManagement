using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Concrete
{
    public class EmployeeRepository : GenericEntity<AssetManagementEntities, Employee>, IEmployeeRepository
    {
        public Employee FindEmployee(string employeeNumber)
        {
            return Context.Employees.FirstOrDefault(x => x.employeeNumber.Equals(employeeNumber));
        }
        public override void Insert(Employee dependent)
        {
            base.Insert(dependent);
        }
        public List<Employee> Employees()
        {
            return Context.Employees.ToList();
        }
        public List<Department> Departments()
        {
            return Context.Departments.ToList();
        }
        public List<Role> Roles()
        {
            return Context.Roles.ToList();
        }
        public Role FindRoles(int? id)
        {
            return Context.Roles.FirstOrDefault(r => r.RoleID == id);
        }
    }
}
