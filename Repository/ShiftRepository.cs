using System;
using System.Linq;
using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
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

        public ShiftDto GetFullShift(IMapper mapper, Guid shiftId, Guid personId)
        {
            var shiftFromDb = this
                .FindByCondition(x => x.Id == shiftId)
                .Include(x => x.Category)
                .Include(x => x.Applications).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                .Include(x => x.Attendees).ThenInclude(x => x.Team)
                .Include(x => x.Attendees).ThenInclude(x => x.Person).ThenInclude(x => x.Congregation)
                .SingleOrDefault();

            var shift = mapper.Map<ShiftDto>(shiftFromDb);

            var application = shift.Applications.SingleOrDefault(x => x.PersonId == personId);
            if (application != null)
            {
                shift.IsApplicant = true;
                shift.IsAttendee = shift.Attendees.Any(x => x.ApplicationId == application.Id);
            }

            return shift;
        }
    }
}
