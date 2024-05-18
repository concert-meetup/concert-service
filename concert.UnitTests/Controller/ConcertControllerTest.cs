using concert.API.Controllers;
using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.DTO.Response;
using concert.API.Models;
using concert.API.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace concert.UnitTests.Controller;

[TestClass]
public class ConcertControllerTest
{
    private Mock<IConcertService> _mockConcertService;
    private ConcertController _controller;

    private const int HTTP_OK = 200;
    private const int HTTP_BAD_REQUEST = 400;
    private const int HTTP_NOT_FOUND = 404;

    [TestInitialize]
    public void Setup()
    {
        _mockConcertService = new Mock<IConcertService>();
        _controller = new ConcertController(_mockConcertService.Object);
    }
    
    [TestMethod]
    public async Task CreateConcert_ShouldReturnOk()
    {
        // arrange
        CreateConcertRequestDTO requestDTO = new CreateConcertRequestDTO
        {
            ConcertDate = DateTime.Today,
            Artist = "Artist",
            ImageUrl = "ImageUrl",
            VenueName = "VenueName",
            City = "City",
            Country = "Country"
        };

        var expectedResponse = new CreateConcertResponseDTO { Id = 1 };

        _mockConcertService.Setup(s => s.CreateConcert(requestDTO)).ReturnsAsync(expectedResponse);
        
        // act
        var result = await _controller.CreateConcert(requestDTO) as OkObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_OK, result.StatusCode);
    }

    [TestMethod]
    public async Task CreateConcert_ShouldReturnBadRequest()
    {
        // arrange
        CreateConcertRequestDTO requestEmpty = new CreateConcertRequestDTO
        {
            ConcertDate = DateTime.Today,
            Artist = "",
            ImageUrl = "ImageUrl",
            VenueName = "VenueName",
            City = "City",
            Country = "Country"
        };
        
        CreateConcertRequestDTO requestNull = new CreateConcertRequestDTO
        {
            ConcertDate = DateTime.Today,
            Artist = null,
            ImageUrl = "ImageUrl",
            VenueName = "VenueName",
            City = "City",
            Country = "Country"
        };

        var errorMsg = "please fill in all details";
        _mockConcertService.Setup(s => s.CreateConcert(requestEmpty)).ThrowsAsync(new ArgumentException(errorMsg));
        _mockConcertService.Setup(s => s.CreateConcert(requestNull)).ThrowsAsync(new ArgumentException(errorMsg));

        // act
        var resultEmpty = await _controller.CreateConcert(requestEmpty) as BadRequestObjectResult;
        var resultNull = await _controller.CreateConcert(requestNull) as BadRequestObjectResult;

        // assert
        Assert.IsNotNull(resultEmpty);
        Assert.AreEqual(HTTP_BAD_REQUEST, resultEmpty.StatusCode);
        Assert.AreEqual(errorMsg, resultEmpty.Value);
        
        Assert.IsNotNull(resultNull);
        Assert.AreEqual(HTTP_BAD_REQUEST, resultNull.StatusCode);
        Assert.AreEqual(errorMsg, resultNull.Value);
    }

    [TestMethod]
    public async Task GetAllConcerts_ShouldReturnOkResponse()
    {
        // arrange
        var expectedResponse = new GetConcertsResponseDTO { };
        _mockConcertService.Setup(s => s.GetAllConcerts(1, 1, null)).ReturnsAsync(expectedResponse);
        
        // act
        var result = await _controller.GetConcerts() as OkObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_OK, result.StatusCode);
    }

    [TestMethod]
    public async Task GetAllConcert_WithSearchQuery_ShouldReturnOkResponse()
    {
        // arrange
        string searchQuery = "Trophy";

        var concerts = new GetConcertsResponseDTO
        {
            Concerts = new HashSet<ConcertSummaryDTO>
            {
                new ConcertSummaryDTO { Artist = "Trophy Eyes" },
                new ConcertSummaryDTO { Artist = "Boston Manor" }
            }
        };
        
        _mockConcertService.Setup(s => s.GetAllConcerts(It.IsAny<int>(), It.IsAny<int>(), searchQuery))
            .ReturnsAsync(concerts);
        
        // act
        var result = await _controller.GetConcerts(1, 15, searchQuery) as OkObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_OK, result.StatusCode);
    }

    [TestMethod]
    public async Task GetConcerts_WithInvalidPage_ShouldReturnBadRequest()
    {
        // arrange
        var errorMsg = "page and pageSize size must be greater than 0.";
        
        // act
        var result = await _controller.GetConcerts(-1, 1, null) as BadRequestObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_BAD_REQUEST, result.StatusCode);
        Assert.AreEqual(errorMsg, result.Value);
    }
    
    [TestMethod]
    public async Task GetConcerts_WithInvalidPageSize_ShouldReturnBadRequest()
    {
        // arrange
        var errorMsg = "page and pageSize size must be greater than 0.";
        
        // act
        var result = await _controller.GetConcerts(1, -1, null) as BadRequestObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_BAD_REQUEST, result.StatusCode);
        Assert.AreEqual(errorMsg, result.Value);
    }

    [TestMethod]
    public async Task GetConcertById_ShouldReturnOkResponse()
    {
        // arrange
        var expectedResponse = new ConcertInfoDTO { };
        _mockConcertService.Setup(s => s.GetConcertById(1)).ReturnsAsync(expectedResponse);
        
        // act
        var result = await _controller.GetConcertById(1) as OkObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_OK, result.StatusCode);
    }

    [TestMethod]
    public async Task GetConcertById_ShouldReturnNotFound()
    {
        // arrange
        _mockConcertService.Setup(s => s.GetConcertById(1))!.ReturnsAsync((ConcertInfoDTO)null!);
        
        // act
        var result = await _controller.GetConcertById(1) as NotFoundResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_NOT_FOUND, result.StatusCode);
    }
}