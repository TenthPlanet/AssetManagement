using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Abstract
{
    public interface IEmployeeRepository : IGenericRepository<Department, Employee>
    {
        Employee FindEmployee(string employeeNumber);
        List<Employee> Employees();
        List<Department> Departments();
    }
}
