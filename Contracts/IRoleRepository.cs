using System;
using Server.Entities.Models;

namespace Server.Contracts
{
    public interface IRoleRepository : IRepositoryBase<Role> {
        Role GetAdministratorRole(Guid projectId);
    }
}
