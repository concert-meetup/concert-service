using concert.API.Models;

namespace concert.API.Data.Abstractions;

public interface IVenueRepository
{
    void Create(Venue venue);
    Task<Venue?> GetVenueByName(string name);
    bool ExistsByName(string name);
    void Update(Venue venue);
    Task<bool> SaveChangesAsync();
}