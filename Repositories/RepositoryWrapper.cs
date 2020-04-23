using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Server.Contracts;
using Server.Entities;

namespace Server.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _user;
        private IPersonRepository _person;
        private IProjectRepository _project;
        private IParticipationRepository _participation;
        private IRoleRepository _role;
        private ICategoryRepository _category;
        private ITeamRepository _team;
        private IShiftRepository _shift;
        private IApplicationRepository _application;
        private IEligibilityRepository _eligibility;
        private ICongregationRepository _congregation;
        private ITopicRepository _topic;
        private IArticleRepository _article;

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

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_repositoryContext);
                }
                return _category;
            }
        }

        public IShiftRepository Shift
        {
            get
            {
                if (_shift == null)
                {
                    _shift = new ShiftRepository(_repositoryContext);
                }
                return _shift;
            }
        }

        public IApplicationRepository Application
        {
            get
            {
                if (_application == null)
                {
                    _application = new ApplicationRepository(_repositoryContext);
                }
                return _application;
            }
        }

        public ITeamRepository Team
        {
            get
            {
                if (_team == null)
                {
                    _team = new TeamRepository(_repositoryContext);
                }
                return _team;
            }
        }

        public IEligibilityRepository Eligibility
        {
            get
            {
                if (_eligibility == null)
                {
                    _eligibility = new EligibilityRepository(_repositoryContext);
                }
                return _eligibility;
            }
        }

        public ICongregationRepository Congregation
        {
            get
            {
                if (_congregation == null)
                {
                    _congregation = new CongregationRepository(_repositoryContext);
                }
                return _congregation;
            }
        }

        public ITopicRepository Topic
        {
            get
            {
                if (_topic == null)
                {
                    _topic = new TopicRepository(_repositoryContext);
                }
                return _topic;
            }
        }

        public IArticleRepository Article
        {
            get
            {
                if (_article == null)
                {
                    _article = new ArticleRepository(_repositoryContext);
                }
                return _article;
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
            using(IDbContextTransaction transaction = _repositoryContext.Database.BeginTransaction())
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
