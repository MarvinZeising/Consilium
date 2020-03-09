using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}

        public bool BelongsToUser(Guid personId, HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.Sid).Value;
            var person = GetById(personId);
            return person?.UserId == new Guid(userId);
        }

        public Person GetById(Guid personId, bool includeParticipations = false)
        {
            var query = FindByCondition(x => x.Id == personId);

            if (includeParticipations)
            {
                query = query
                    .Include(x => x.Participations).ThenInclude(x => x.Project).ThenInclude(x => x.Topics)
                    .Include(x => x.Participations).ThenInclude(x => x.Role).ThenInclude(x => x.Eligibilities);
            }

            return query.SingleOrDefault();
        }

    }
}
