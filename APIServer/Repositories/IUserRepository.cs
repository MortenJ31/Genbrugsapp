using Core;
using System.Threading.Tasks;


namespace APIServer.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
    }
}
