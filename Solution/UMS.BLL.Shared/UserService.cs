using UMS.DAL.Shared;
using UMS.Models;
using UMS.DAL;
using System.Collections.Generic;

namespace UMS.BLL.Shared
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService()
        {
            _repo = new UserRepository();
        }

        public List<User> GetUsers() => _repo.GetAll();

        public bool RegisterUser(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                return false;

            var user = new User { Name = name, Email = email };
            return _repo.Add(user);
        }
    }
}
