using APIServer.Services;
using Core;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(MongoDbService mongoDbService)
        {
            _usersCollection = mongoDbService.Users;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _usersCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }
    }
}