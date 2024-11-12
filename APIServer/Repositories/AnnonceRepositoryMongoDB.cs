using MongoDB.Driver;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class AnnonceRepositoryMongoDB : IAnnonceRepository
    {
        private readonly IMongoCollection<Annonce> _collection;

        public AnnonceRepositoryMongoDB()
        {
            var mongoUri = "mongodb+srv://mortenjeppesen91:DWPZGhvDJz2NPIrH@devcluster.mppws.mongodb.net/?retryWrites=true&w=majority&appName=DevCluster";

            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("DevCluster");
            _collection = database.GetCollection<Annonce>("Ad");
        }

        public async Task<List<Annonce>> GetAnnoncerAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task AddAnnonceAsync(Annonce annonce)
        {
            await _collection.InsertOneAsync(annonce);
        }

        public async Task<bool> UpdateAnnonceStatusAsync(string id, string newStatus)
        {
            var filter = Builders<Annonce>.Filter.Eq(a => a.Id, id);
            var update = Builders<Annonce>.Update.Set(a => a.Status, newStatus);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}