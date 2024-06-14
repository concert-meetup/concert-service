using System.Net;
using System.Text;
using concert.API.DTO;
using concert.API.DTO.Request;
using concert.IntegrationTest.Config;
using FluentAssertions;
using Newtonsoft.Json;

namespace concert.IntegrationTest;

public class ConcertControllerTest : IClassFixture<ConcertServiceWebAppFactory>
{
    private readonly HttpClient _client;

    public ConcertControllerTest(ConcertServiceWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetConcerts_GetsAllConcerts()
    {
        // act   
        var response = await _client.GetAsync("/api/Concert");
        
        // assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("Trophy Eyes");
        responseString.Should().Contain("Boston Manor");
    }
    
    [Fact]
    public async Task GetConcerts_GetsAllConcerts_WithPageSize1()
    {
        // act   
        var response = await _client.GetAsync("/api/Concert?page=1&pageSize=1");
        
        // assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("Trophy Eyes");
        responseString.Should().NotContain("Boston Manor");
    }
    
    [Fact]
    public async Task GetConcerts_InvalidPageParameters_ReturnsBadRequest()
    {
        // arrange
        var page = 0;
        var pageSize = 15;

        // act
        var response = await _client.GetAsync($"/api/concert?page={page}&pageSize={pageSize}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("page and pageSize size must be greater than 0.");
    }
    
    [Fact]
    public async Task AddConcert_ShouldCreateConcert()
    {
        // arrange
        var request = new CreateConcertRequestDTO()
        {
            Artist = "Stand Atlantic",
            ConcertDate = DateTime.Today.AddDays(10),
            VenueName = "Project House",
            City = "Leeds",
            Country = "England"
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // act
        var response = await _client.PostAsync("/api/concert", content);

        // assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("3");
    }
    
    [Fact]
    public async Task AddConcert_MissingFields_ShouldThrowException()
    {
        // arrange
        var request = new CreateConcertRequestDTO()
        {
            ConcertDate = DateTime.Today.AddDays(10),
            VenueName = "Project House",
            City = "Leeds",
            Country = "England"
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // act
        var response = await _client.PostAsync("/api/concert", content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }
    
    [Fact]
    public async Task GetConcertById_ExistingId_ReturnsConcert()
    {
        // arrange
        var concertId = 1;

        // act
        var response = await _client.GetAsync($"/api/concert/{concertId}");

        // assert
        response.EnsureSuccessStatusCode();
        var concert = JsonConvert.DeserializeObject<ConcertInfoDTO>(await response.Content.ReadAsStringAsync());
        concert.Should().NotBeNull();
        concert?.Id.Should().Be(concertId);
    }
    
    [Fact]
    public async Task GetConcertById_NonExistingId_ReturnsNotFound()
    {
        // arrange
        var concertId = 999;

        // act
        var response = await _client.GetAsync($"/api/concert/{concertId}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}