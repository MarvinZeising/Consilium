using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class AttendeeRepository : RepositoryBase<Attendee>, IAttendeeRepository
    {
        public AttendeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}
    }
}
