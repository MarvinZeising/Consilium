using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ShiftRepository : RepositoryBase<Shift>, IShiftRepository
    {
        public ShiftRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
