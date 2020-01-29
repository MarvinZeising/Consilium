using System;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IShiftRepository : IRepositoryBase<Shift>
    {
        ShiftDto GetFullShift(IMapper mapper, Guid shiftId, Guid personId);
    }
}
