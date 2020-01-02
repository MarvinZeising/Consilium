using System;
using Entities.Models;

namespace Contracts
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        Project GetById(Guid projectId, bool includeParticipants = false);
    }
}
