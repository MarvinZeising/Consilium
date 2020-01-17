using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class EligibilityRepository : RepositoryBase<Eligibility>, IEligibilityRepository
    {
        public EligibilityRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
