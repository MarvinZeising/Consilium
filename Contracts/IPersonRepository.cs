using System;
using Entities.Models;

namespace Contracts
{
    public interface IPersonRepository : IRepositoryBase<Person>
    {
        Person GetPersonById(Guid id, bool includeParticipations = false);
    }
}
