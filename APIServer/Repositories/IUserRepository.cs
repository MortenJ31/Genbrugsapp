using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task AddUserAsync(User newUser);
        Task UpdateUserAsync(string id, User updatedUser);
        Task DeleteUserAsync(string id);
        Task<List<User>> GetUsersBySearchQueryAsync(string searchQuery);
    }
}