using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.DTO.Response;

namespace concert.API.Service.Abstractions;

public interface IConcertService
{
    Task<CreateConcertResponseDTO> CreateConcert(CreateConcertRequestDTO request);
    Task<GetConcertsResponseDTO> GetAllConcerts(int page, int pageSize, string searchQuery);
    // Task<IEnumerable<Concert>> GetConcertByName(string name, int skip, int pageSize);
    Task<GetConcertsResponseDTO> GetConcertsByName(int page, int pageSize, string searchQuery);
    Task<ConcertInfoDTO> GetConcertById(int id);
}