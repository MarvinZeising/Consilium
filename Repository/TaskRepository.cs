using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class TaskRepository : RepositoryBase<Task>, ITaskRepository
    {
        public TaskRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
