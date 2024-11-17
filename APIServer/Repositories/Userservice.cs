using Core;
using APIServer.Repositories;

namespace APIServer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<User>> GetAllUsersAsync() => _userRepository.GetAllUsersAsync();

        public Task<User?> GetUserByIdAsync(string id) => _userRepository.GetUserByIdAsync(id);

        public Task AddUserAsync(User newUser) => _userRepository.AddUserAsync(newUser);

        public Task UpdateUserAsync(string id, User updatedUser) => _userRepository.UpdateUserAsync(id, updatedUser);

        public Task DeleteUserAsync(string id) => _userRepository.DeleteUserAsync(id);
        
        public Task<User?> GetUserByUsernameAsync(string username) => _userRepository.GetUserByUsernameAsync(username);
    }
}