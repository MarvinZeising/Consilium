using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class TopicRepository : RepositoryBase<Topic>, ITopicRepository
    {
        public TopicRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
