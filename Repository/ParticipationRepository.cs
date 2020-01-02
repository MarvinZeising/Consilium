using System.Linq;
using System;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ParticipationRepository : RepositoryBase<Participation>, IParticipationRepository
    {
        public ParticipationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}
