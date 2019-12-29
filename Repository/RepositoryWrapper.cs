using Contracts;
using Entities;
using Microsoft.Extensions.Configuration;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IUserRepository _user;
        private IPersonRepository _person;

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
            _repositoryContext.SaveChanges();
        }
    }
}
