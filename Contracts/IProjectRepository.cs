using System;
using Server.Entities.Models;

namespace Server.Contracts
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        Project GetById (Guid projectId, bool includeParticipants = false);
    }
}
