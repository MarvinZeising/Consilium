using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
