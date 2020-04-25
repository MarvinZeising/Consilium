using System;
using System.Linq;
using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}

        public Role GetAdministratorRole(Guid projectId) {
            return FindByCondition(x => x.ProjectId == projectId && !x.Editable).SingleOrDefault();
        }
    }
}
