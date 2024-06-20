using concert.API.Data;
using concert.API.Models;

namespace concert.IntegrationTest.Config;

public static class ConcertDbContextDataSeed
{
    public static void SeedTestData(ConcertDbContext context)
    {
        if (!context.Concerts.Any())
        {
            context.Concerts.AddRange(
                new Concert
                {
                    Id = 1,
                    Artist = "Trophy Eyes",
                    ConcertDate = DateTime.Today.AddDays(10),
                    Venue = new Venue
                    {
                        Id = 1,
                        Name = "Dynamo",
                        City = "Eindhoven",
                        Country = "The Netherlands",
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ImageUrl = "image_url",
                    VenueId = 1
                },
                new Concert
                {
                    Id = 2,
                    Artist = "Boston Manor",
                    ConcertDate = DateTime.Today.AddDays(10),
                    Venue = new Venue
                    {
                        Id = 2,
                        Name = "SWG3",
                        City = "Glasgow",
                        Country = "Scotland",
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    },
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ImageUrl = "image_url",
                    VenueId = 2
                });

            context.SaveChanges();
        }
    }
}