using Core;
using System.Threading.Tasks;

namespace Genbrugsapp.Service
{
    public interface ILoginService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);  
        Task<User?> GetUserLoggedIn();
        Task Logout();
        Task<string> GetUserId();
        Task SetUserId(string userId);
    }
}