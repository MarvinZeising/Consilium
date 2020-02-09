using System;
using AutoMapper;
using Server.Entities.DataTransferObjects;
using Server.Entities.Models;

namespace Server.Contracts
{
    public interface IShiftRepository : IRepositoryBase<Shift>
    {
        ShiftDto GetFullShift (IMapper mapper, Guid shiftId, Guid personId);
    }
}
