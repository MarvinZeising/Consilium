using System;
using Entities.Models;

namespace Contracts
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        Person GetById(Guid id, bool includeParticipations = false);
        bool BelongsToUser(Guid personId, string userId);
    }
}
