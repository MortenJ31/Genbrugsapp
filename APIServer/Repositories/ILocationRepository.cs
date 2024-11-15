using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(string id);
        Task AddLocationAsync(Location location);
        Task UpdateLocationAsync(string id, Location updatedLocation);
        Task DeleteLocationAsync(string id);
    }
}