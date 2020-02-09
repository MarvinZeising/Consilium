using System;
using Server.Entities.Models;

namespace Server.Contracts
{
    public interface IParticipationRepository : IRepositoryBase<Participation>
    {
        Participation GetParticipation (Guid personId, Guid projectId);
        Role GetRole (Guid personId, Guid projectId);
        Eligibility GetEligibilityByCategory (Guid personId, Guid projectId, Guid categoryId);
    }
}
