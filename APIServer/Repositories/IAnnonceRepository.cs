using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace APIServer.Repositories
{
    public interface IAnnonceRepository
    {
        Task<List<Annonce>> GetAnnoncerAsync();
        Task AddAnnonceAsync(Annonce annonce);
        Task<bool> UpdateAnnonceStatusAsync(string id, string newStatus);
    }
}