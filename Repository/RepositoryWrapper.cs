using System;
using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _user;
        private IPersonRepository _person;
        private IProjectRepository _project;
        private IParticipationRepository _participation;
        private IRoleRepository _role;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repositoryContext, Configuration);
                }
                return _user;
            }
        }

        public IPersonRepository Person
        {
            get
            {
                if (_person == null)
                {
                    _person = new PersonRepository(_repositoryContext);
                }
                return _person;
            }
        }

        public IProjectRepository Project
        {
            get
            {
                if (_project == null)
                {
                    _project = new ProjectRepository(_repositoryContext);
                }
                return _project;
            }
        }

        public IParticipationRepository Participation
        {
            get
            {
                if (_participation == null)
                {
                    _participation = new ParticipationRepository(_repositoryContext);
                }
                return _participation;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_repositoryContext);
                }
                return _role;
            }
        }

        public IConfiguration Configuration { get; }

        public RepositoryWrapper(
            RepositoryContext repositoryContext,
            IConfiguration configuration)
        {
            _repositoryContext = repositoryContext;
            Configuration = configuration;
        }

        public void Save()
        {
            using (IDbContextTransaction transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    _repositoryContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
    }
}
