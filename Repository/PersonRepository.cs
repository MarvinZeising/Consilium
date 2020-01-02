using System.Linq;
using System;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool BelongsToUser(Guid personId, string userId)
        {
            var person = GetById(personId);
            return person?.UserId == new Guid(userId);
        }

        public Person GetById(Guid personId, bool includeParticipations = false)
        {
            var query = FindByCondition(x => x.Id == personId);

            if (includeParticipations) {
                query = query.Include(x => x.Participations);
            }

            return query.SingleOrDefault();
        }
    }
}
