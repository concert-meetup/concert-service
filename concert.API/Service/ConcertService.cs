using AutoMapper;
using concert.API.Data.Abstractions;
using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.DTO.Response;
using concert.API.Models;
using concert.API.Service.Abstractions;

namespace concert.API.Service;

public class ConcertService : IConcertService
{
    private readonly IConcertRepository _concertRepository;
    private readonly IVenueRepository _venueRepository;

    private readonly IMapper _mapper;

    public ConcertService(IConcertRepository concertRepository, IVenueRepository venueRepository, IMapper mapper)
    {
        _concertRepository = concertRepository;
        _venueRepository = venueRepository;
        _mapper = mapper;
    }

    public async Task<CreateConcertResponseDTO> CreateConcert(CreateConcertRequestDTO request)
    {
        if (string.IsNullOrEmpty(request.Artist) ||
            string.IsNullOrEmpty(request.VenueName) || string.IsNullOrEmpty(request.City) ||
            string.IsNullOrEmpty(request.Country))
        {
            throw new ArgumentException("please fill in all details");
        }

        if (string.IsNullOrEmpty(request.ImageUrl))
        {
            request.ImageUrl = "default image url";
        }

        Venue venue;
        if (!_venueRepository.ExistsByName(request.VenueName))
        {
            venue = new Venue
            {
                Name = request.VenueName,
                City = request.City,
                Country = request.Country,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            await SaveVenue(venue);
        }

        venue = await _venueRepository.GetVenueByName(request.VenueName);

        Concert newConcert = new Concert
        {
            ConcertDate = request.ConcertDate,
            Artist = request.Artist,
            ImageUrl = request.ImageUrl,
            Venue = venue,
            VenueId = venue.Id,
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

        Concert savedConcert = await SaveConcert(newConcert);

        return new CreateConcertResponseDTO
        {
            Id = savedConcert.Id
        };
    }

    private async Task<Concert> SaveConcert(Concert concert)
    {
        _concertRepository.Create(concert);
        await _concertRepository.SaveChangesAsync();
        return concert;
    }

    private async Task<Venue> SaveVenue(Venue venue)
    {
        _venueRepository.Create(venue);
        await _venueRepository.SaveChangesAsync();
        return venue;
    }

    public async Task<GetConcertsResponseDTO> GetAllConcerts()
    {
        var concerts = await _concertRepository.GetAll();
        var concertSummaryDtOs = _mapper.Map<IEnumerable<ConcertSummaryDTO>>(concerts);
        return new GetConcertsResponseDTO
        {
            Concerts = new HashSet<ConcertSummaryDTO>(concertSummaryDtOs)
        };
    }

    public async Task<ConcertInfoDTO> GetConcertById(int id)
    {
        var concert = await _concertRepository.GetById(id);
        var concertInfoDto = _mapper.Map<ConcertInfoDTO>(concert);

        return concertInfoDto;
    }
}