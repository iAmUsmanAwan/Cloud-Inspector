using UMS.DAL.Shared;
using UMS.Models;
using UMS.DAL;
using System.Collections.Generic;

namespace UMS.BLL
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> RegisterUserAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
