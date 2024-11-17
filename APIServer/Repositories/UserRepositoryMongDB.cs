using MongoDB.Bson;
using MongoDB.Driver;
using Core;


namespace APIServer.Repositories
{
    public class UserRepositoryMongoDB : IUserRepository
    {
        private readonly IMongoCollection<BsonDocument> _users;

        public UserRepositoryMongoDB(IMongoDatabase database)
        {
            _users = database.GetCollection<BsonDocument>("Users");
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var documents = await _users.Find(_ => true).ToListAsync();
            return documents.ConvertAll(ToUser);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var document = await _users.Find(filter).FirstOrDefaultAsync();

            return document != null ? ToUser(document) : null;
        }

        public async Task AddUserAsync(User newUser)
        {
            var document = new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId() },
                { "Username", newUser.Username },
                { "Password", newUser.Password },
                { "CreatedAt", newUser.CreatedAt }
            };

            await _users.InsertOneAsync(document);
        }

        public async Task UpdateUserAsync(string id, User updatedUser)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var document = new BsonDocument
            {
                { "Username", updatedUser.Username },
                { "Password", updatedUser.Password },
                { "CreatedAt", updatedUser.CreatedAt }
            };

            await _users.ReplaceOneAsync(filter, document);
        }

        public async Task DeleteUserAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            await _users.DeleteOneAsync(filter);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var document = await _users.Find(filter).FirstOrDefaultAsync();

            return document != null ? ToUser(document) : null;
        }

        private User ToUser(BsonDocument document)
        {
            return new User
            {
                UserId = document["_id"].ToString(),
                Username = document["Username"].AsString,
                Password = document["Password"].AsString,
                CreatedAt = document["CreatedAt"].ToUniversalTime()
            };
        }
    }
}
