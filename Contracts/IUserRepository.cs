using System;
using Entities.Models;

namespace Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        string Authenticate(string email, string password);
        User Register(User user, string password);
        bool ChangePassword(Guid userId, string oldPassword, string newPassword);
        User GetUserById(Guid id, bool includePersons = false);
    }
}
