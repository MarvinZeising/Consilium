using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class ParticipationRepository : RepositoryBase<Participation>, IParticipationRepository
    {
        public ParticipationRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}

        public Participation GetParticipation(Guid personId, Guid projectId)
        {
            return FindByCondition(x => x.PersonId == personId && x.ProjectId == projectId)
                .Include(x => x.Role).ThenInclude(x => x.Eligibilities)
                .SingleOrDefault();
        }

        public Role GetRole(Guid personId, Guid projectId)
        {
            return GetParticipation(personId, projectId)?.Role;
        }

        public Eligibility GetEligibilityByCategory(Guid personId, Guid projectId, Guid categoryId)
        {
            return GetRole(personId, projectId)?.Eligibilities.Where(x => x.CategoryId == categoryId).SingleOrDefault();
        }

    }
}
