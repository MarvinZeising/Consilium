using System;
using Entities.Models;

namespace Contracts
{
    public interface IShiftRepository : IRepositoryBase<Shift>
    {
        Shift GetFullShift(Guid shiftId);
    }
}
