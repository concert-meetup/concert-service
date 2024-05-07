using concert.API.Controllers;
using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.DTO.Response;
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
    public async Task GetAllConcerts_ShouldReturnOkResponse()
    {
        // arrange
        var expectedResponse = new GetConcertsResponseDTO { };
        _mockConcertService.Setup(s => s.GetAllConcerts()).ReturnsAsync(expectedResponse);
        
        // act
        var result = await _controller.GetConcerts() as OkObjectResult;
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HTTP_OK, result.StatusCode);
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