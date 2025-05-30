using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.DAL.Shared
{
    public interface IUserRepository
    {
        List<User> GetAll();
        bool Add(User user);
    }
}
