using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Concrete
{
    public class DepartmentRepository : GenericEntity<AssetManagementEntities, Department>, IDepartmentRepository
    {
        public override void Insert(Department entity)
        {
            base.Insert(entity);
            Save();
        }
        public override void Delete(Department entity)
        {
            base.Delete(entity);
            Save();
        }
        public override void Update(Department entity)
        {
            base.Update(entity);
            Save();
        }
        public Department FindDepartment(int? id)
        {
            return Context.Departments.Find(id);
        }
        public List<Department> GetAll()
        {
            return Context.Departments.ToList();
        }
    }
}
