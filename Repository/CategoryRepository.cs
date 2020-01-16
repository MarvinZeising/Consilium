using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
