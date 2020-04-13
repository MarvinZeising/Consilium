using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class CongregationRepository : RepositoryBase<Congregation>, ICongregationRepository
    {
        public CongregationRepository(RepositoryContext repositoryContext) : base(repositoryContext) {}

    }
}
