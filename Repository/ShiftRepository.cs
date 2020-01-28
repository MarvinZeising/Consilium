using System;
using System.Linq;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ShiftRepository : RepositoryBase<Shift>, IShiftRepository
    {
        public ShiftRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Shift GetFullShift(Guid shiftId)
        {
            return this
                .FindByCondition(x => x.Id == shiftId)
                .Include(x => x.Category)
                .Include(x => x.Applications).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                .Include(x => x.Attendees).ThenInclude(x => x.Team)
                .Include(x => x.Attendees).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                .SingleOrDefault();
        }
    }
}
