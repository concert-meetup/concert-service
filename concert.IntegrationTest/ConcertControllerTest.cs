using System.Net.Http.Json;
using concert.API.DTO.Request;

namespace concert.IntegrationTest;

public class ConcertControllerTest
{
    [Fact]
    public async Task CreateConcert_SavesConcert()
    {
        // arrange
        var app = new ConcertServiceWebAppFactory();

        CreateConcertRequestDTO request = new CreateConcertRequestDTO
        {
            Artist = "Trophy Eyes",
            ConcertDate = DateTime.Today,
            VenueName = "Dynamo",
            City = "Eindhoven",
            Country = "The Netherlands"
        };

        var client = app.CreateClient();

        // act
        var response = await client.PostAsJsonAsync("/api/concert", request);

        // assert
        response.EnsureSuccessStatusCode();
    }
}