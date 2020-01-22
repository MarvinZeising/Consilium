using System;
using Entities.Models;

namespace Contracts
{
    public interface IParticipationRepository : IRepositoryBase<Participation>
    {
        Participation GetParticipation(Guid personId, Guid projectId);
        Role GetRole(Guid personId, Guid projectId);
        Eligibility GetEligibilityByCategory(Guid personId, Guid projectId, Guid categoryId);
    }
}
