using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CongregationRepository : RepositoryBase<Congregation>, ICongregationRepository
    {
        public CongregationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}
