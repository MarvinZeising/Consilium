using System;
using Server.Entities.Models;

namespace Server.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        string Authenticate(string email, string password);
        User Register(User user, string password);
        bool ChangePassword(Guid userId, string oldPassword, string newPassword);
        User GetById(Guid id, bool includePersons = false);
    }
}
