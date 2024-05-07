using concert.API.Models;

namespace concert.API.Data.Abstractions;

public interface IConcertRepository
{
    void Create(Concert concert);
    Task<IEnumerable<Concert>> GetAll();
    Task<Concert?> GetById(int id);
    bool Update(Concert concert);
    bool Delete(Concert concert);
    bool ExistsById(int id);
    Task<bool> SaveChangesAsync();
}