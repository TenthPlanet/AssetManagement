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
    public class RoleRepository : GenericEntity<AssetManagementEntities, Role>, IRoleRepository
    {
        public List<Role> GetRoles()
        {
            return Context.Roles.ToList();
        }
        public Role FindRole(int? id)
        {
            return Context.Roles.Find(id);
        }
    }
}
