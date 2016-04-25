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
    public class EmployeeRepository : GenericRepository<AssetManagementEntities, Department, Employee>, IEmployeeRepository
    {
        public Employee FindEmployee(string employeeNumber)
        {
            return Context.Employees.FirstOrDefault(x => x.employeeNumber.Equals(employeeNumber));
        }

        public override void Insert(Department entity, Employee dependent)
        {
            base.Insert(entity, dependent);
        }
        public List<Employee> Employees()
        {
            return Context.Employees.ToList();
        }

        public List<Department> Departments()
        {
            return Context.Departments.ToList();
        }

    }
}
