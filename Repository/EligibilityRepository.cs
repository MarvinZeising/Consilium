using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class EligibilityRepository : RepositoryBase<Eligibility>, IEligibilityRepository
    {
        public EligibilityRepository (RepositoryContext repositoryContext) : base (repositoryContext) { }
    }
}
