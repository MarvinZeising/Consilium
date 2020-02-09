using System;
using Microsoft.AspNetCore.Http;
using Server.Entities.Models;

namespace Server.Contracts
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        Person GetById (Guid id, bool includeParticipations = false);
        bool BelongsToUser (Guid personId, HttpContext context);
    }
}
