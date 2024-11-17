using Core;


public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task AddUserAsync(User newUser);
    Task UpdateUserAsync(string id, User updatedUser);
    Task DeleteUserAsync(string id);
    Task<User?> GetUserByUsernameAsync(string username);
}