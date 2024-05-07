using concert.API.Data.Abstractions;
using concert.API.Models;
using Microsoft.EntityFrameworkCore;

namespace concert.API.Data;

public class VenueRepository : IVenueRepository
{
    private readonly ConcertDbContext _context;

    public VenueRepository(ConcertDbContext context)
    {
        _context = context;
    }

    public void Create(Venue venue)
    {
        ArgumentNullException.ThrowIfNull(venue);
        _context.Venues.Add(venue);
    }

    public async Task<Venue?> GetVenueByName(string name)
    {
        return await _context.Venues.FirstOrDefaultAsync(v => v.Name == name);
    }

    public bool ExistsByName(string name)
    {
        return _context.Venues.Any(v => v.Name == name);
    }

    public void Update(Venue venue)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}