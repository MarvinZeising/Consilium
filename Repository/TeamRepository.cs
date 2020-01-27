using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
