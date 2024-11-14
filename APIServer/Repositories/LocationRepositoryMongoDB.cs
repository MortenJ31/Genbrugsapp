using MongoDB.Driver;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class LocationRepositoryMongoDB : ILocationRepository
    {
        private readonly IMongoCollection<Location> _locations;

        public LocationRepositoryMongoDB(IMongoDatabase database)
        {
            _locations = database.GetCollection<Location>("Location");
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _locations.Find(loc => true).ToListAsync();
        }

        public async Task<Location> GetLocationByIdAsync(string id)
        {
            return await _locations.Find(loc => loc.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddLocationAsync(Location location)
        {
            location.Id = null;
            await _locations.InsertOneAsync(location);
        }

        public async Task UpdateLocationAsync(string id, Location updatedLocation)
        {
            await _locations.ReplaceOneAsync(loc => loc.Id == id, updatedLocation);
        }

        public async Task DeleteLocationAsync(string id)
        {
            await _locations.DeleteOneAsync(loc => loc.Id == id);
        }
    }
}