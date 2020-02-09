using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Entities;
using Server.Entities.Models;

namespace Server.Repository
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository (RepositoryContext repositoryContext) : base (repositoryContext) { }

        public Project GetById (Guid projectId, bool includeParticipants = false)
        {
            var query = FindByCondition (x => x.Id == projectId);

            if (includeParticipants)
            {
                query = query.Include (x => x.Participants);
            }

            return query.SingleOrDefault ();
        }
    }
}
