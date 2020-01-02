using System.Linq;
using System;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Project GetById(Guid projectId, bool includeParticipants = false)
        {
            var query = FindByCondition(x => x.Id == projectId);

            if (includeParticipants) {
                query = query.Include(x => x.Participants);
            }

            return query.SingleOrDefault();
        }
    }
}
