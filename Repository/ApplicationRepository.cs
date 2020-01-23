using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
