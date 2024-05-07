using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace concert.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcertController : ControllerBase
{
    private readonly IConcertService _concertService;

    public ConcertController(IConcertService concertService)
    {
        _concertService = concertService;
    }

    [HttpPost]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateConcert([FromBody] CreateConcertRequestDTO request)
    {
        try
        {
            var response = await _concertService.CreateConcert(request);
            // var location = new Uri($"{Request.Scheme}://{Request.Host}/api/Concert/{response.Id}");
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetConcerts()
    {
        return Ok(await _concertService.GetAllConcerts());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<ConcertInfoDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConcertById(int id)
    {
        var concert = await _concertService.GetConcertById(id);
        return concert == null ? NotFound() : Ok(concert);
    }
}