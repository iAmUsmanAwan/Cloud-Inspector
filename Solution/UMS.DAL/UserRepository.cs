using UMS.Models;
using UMS.DAL.Shared;
using System.Collections.Generic;

namespace UMS.DAL
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _users = new();

        public List<User> GetAll() => _users;

        public bool Add(User user)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);
            return true;
        }
    }
}
