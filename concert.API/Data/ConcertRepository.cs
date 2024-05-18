using concert.API.Data.Abstractions;
using concert.API.Models;
using Microsoft.EntityFrameworkCore;

namespace concert.API.Data;

public class ConcertRepository : IConcertRepository
{
    private readonly ConcertDbContext _context;

    public ConcertRepository(ConcertDbContext context)
    {
        _context = context;
    }

    public void Create(Concert concert)
    {
        ArgumentNullException.ThrowIfNull(concert);
        _context.Concerts.Add(concert);
    }

    public async Task<IEnumerable<Concert>> GetAll(int skip, int pageSize, string searchQuery)
    {
        // TODO update pagination to keyset for better performance

        var query = _context.Concerts.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(c => c.Artist.Contains(searchQuery));
        }
        else
        {
            query = query.Where(c => c.ConcertDate >= DateTime.Today);
        }

        var concerts = await query
            .Include(c => c.Venue)
            .OrderBy(c => c.ConcertDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return concerts;
    }

    public async Task<Concert?> GetById(int id)
    {
        return await _context.Concerts
            .Include(c => c.Venue)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public bool Update(Concert concert)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Concert concert)
    {
        throw new NotImplementedException();
    }

    public bool ExistsById(int id)
    {
        return _context.Concerts.Any(c => c.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}