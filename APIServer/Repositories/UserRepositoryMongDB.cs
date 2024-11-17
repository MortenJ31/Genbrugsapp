using MongoDB.Driver;
using MongoDB.Bson;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class UserRepositoryMongoDB : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepositoryMongoDB(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public Task<List<User>> GetAllUsersAsync() =>
            _users.Find(user => true).ToListAsync();  // Hent alle brugere

        public Task<User?> GetUserByIdAsync(string id) =>
            _users.Find(user => user.Id == id).FirstOrDefaultAsync();  // Find bruger med specifikt id

        public Task AddUserAsync(User newUser)
        {
            newUser.Id = ObjectId.GenerateNewId().ToString();  // Generer ObjectId som string
            return _users.InsertOneAsync(newUser);  // Indsæt den nye bruger i databasen
        }

        public Task UpdateUserAsync(string id, User updatedUser) =>
            _users.ReplaceOneAsync(user => user.Id == id, updatedUser);  // Erstat den eksisterende bruger

        public Task DeleteUserAsync(string id) =>
            _users.DeleteOneAsync(user => user.Id == id);  // Slet brugeren baseret på id

        public Task<List<User>> GetUsersBySearchQueryAsync(string searchQuery) =>
            _users.Find(user => user.Username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();  // Hent brugere baseret på søgeord i brugernavn
    }
}