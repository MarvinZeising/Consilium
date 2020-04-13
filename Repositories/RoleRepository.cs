using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}
    }
}
