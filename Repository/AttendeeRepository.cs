using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class AttendeeRepository : RepositoryBase<Attendee>, IAttendeeRepository
    {
        public AttendeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
