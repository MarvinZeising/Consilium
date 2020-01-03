using System;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Contracts
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        Person GetById(Guid id, bool includeParticipations = false);
        bool BelongsToUser(Guid personId, HttpContext context);
    }
}
