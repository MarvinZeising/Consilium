using System.Linq;
using System;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Entities.Enums;

namespace Repository
{
    public class ParticipationRepository : RepositoryBase<Participation>, IParticipationRepository
    {
        public ParticipationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Participation GetParticipation(Guid personId, Guid projectId) {
            return FindByCondition(x => x.PersonId == personId && x.ProjectId == projectId)
                .Include(x => x.Role)
                .SingleOrDefault();
        }

        public Role GetRole(Guid personId, Guid projectId) {
            return GetParticipation(personId, projectId)?.Role;
        }

    }
}
