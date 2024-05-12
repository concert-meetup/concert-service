using AutoMapper;
using concert.API.Data.Abstractions;
using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.DTO.Response;
using concert.API.Models;
using concert.API.Service;
using Moq;

namespace concert.UnitTests.Service;

[TestClass]
public class ConcertServiceTest
{
    private Mock<IConcertRepository> _mockConcertRepo;
    private Mock<IVenueRepository> _mockVenueRepo;
    private Mock<IMapper> _mockMapper;

    private ConcertService _concertService;

    [TestInitialize]
    public void Setup()
    {
        _mockConcertRepo = new Mock<IConcertRepository>();
        _mockVenueRepo = new Mock<IVenueRepository>();
        _mockMapper = new Mock<IMapper>();

        _concertService = new ConcertService(_mockConcertRepo.Object, _mockVenueRepo.Object, _mockMapper.Object);
    }

    [TestMethod]
    public async Task CreateConcert_ShouldSaveNewConcert()
    {
        // arrange
        CreateConcertRequestDTO request = new CreateConcertRequestDTO
        {
            ConcertDate = DateTime.Today,
            Artist = "Artist",
            ImageUrl = "ImageUrl",
            VenueName = "VenueName",
            City = "City",
            Country = "Country"
        };
        
        var expectedConcertId = 1;
        var expectedResponse = new CreateConcertResponseDTO { Id = expectedConcertId };

        _mockVenueRepo.Setup(repo => repo.ExistsByName(It.IsAny<string>())).Returns(false);
        _mockVenueRepo.Setup(repo => repo.GetVenueByName(It.IsAny<string>())).ReturnsAsync(new Venue());
        
        // act
        var result =  _concertService.CreateConcert(request);
        
        // assert
        Assert.AreEqual(expectedResponse.Id, result.Id);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(await result, typeof(CreateConcertResponseDTO));
        
        _mockConcertRepo.Verify(r => r.Create(It.IsAny<Concert>()), Times.Once);
        _mockConcertRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        
        _mockVenueRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mockVenueRepo.Verify(r=>r.ExistsByName(It.IsAny<string>()), Times.Once);
        _mockVenueRepo.Verify(r=>r.GetVenueByName(It.IsAny<string>()), Times.Once);        
        _mockVenueRepo.Verify(r => r.Create(It.IsAny<Venue>()), Times.Once);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task CreateConcert_ShouldSaveNewConcert_ShouldThrowWhenNotAllDetailsAreEntered()
    {
        // arrange
        CreateConcertRequestDTO request = new CreateConcertRequestDTO
        {
            
        };
        
        // act
        await _concertService.CreateConcert(request);
    }

    [TestMethod]
    public async Task GetAllConcerts_ShouldReturnListOfConcertSummaries()
    {
        // arrange 
        var expectedConcerts = new List<Concert> { new Concert(), new Concert() };
        var expectedResponse = new GetConcertsResponseDTO
        {
            Concerts = new HashSet<ConcertSummaryDTO> { new ConcertSummaryDTO(), new ConcertSummaryDTO() }
        };

        _mockConcertRepo.Setup(r => r.GetAll(1, 1, "search")).ReturnsAsync(expectedConcerts);
        _mockMapper.Setup(m => m.Map<IEnumerable<ConcertSummaryDTO>>(It.IsAny<IEnumerable<Concert>>()))
            .Returns(expectedResponse.Concerts);
        
        // act
        var result = await _concertService.GetAllConcerts(1, 1, "search");

        // assert
        Assert.IsInstanceOfType<GetConcertsResponseDTO>(result);
        Assert.AreEqual(expectedResponse.Concerts.Count(), result.Concerts.Count());
        
        foreach (var expectedConcert in expectedResponse.Concerts)
        {
            Assert.IsTrue(result.Concerts.Contains(expectedConcert));
        }
    }

    [TestMethod]
    public async Task GetConcertById_ShouldReturnConcertInfo()
    {
        // arrange
        var expectedConcert = new Concert { Artist = "Trophy Eyes" };
        var expectedResponse = new ConcertInfoDTO { Artist = "Trophy Eyes" };

        _mockConcertRepo.Setup(r => r.GetById(1)).ReturnsAsync(expectedConcert);
        _mockMapper.Setup(m => m.Map<ConcertInfoDTO>(It.IsAny<Concert>()))
            .Returns(expectedResponse);
        
        // act 
        var result = await _concertService.GetConcertById(1);
        
        // assert
        Assert.IsInstanceOfType<ConcertInfoDTO>(result);
        Assert.AreEqual(expectedResponse.Artist, result.Artist);
    }
}