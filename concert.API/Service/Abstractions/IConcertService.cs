using concert.API.DTO;
using concert.API.DTO.Request;
using concert.API.DTO.Response;

namespace concert.API.Service.Abstractions;

public interface IConcertService
{
    Task<CreateConcertResponseDTO> CreateConcert(CreateConcertRequestDTO request);
    Task<GetConcertsResponseDTO> GetAllConcerts();
    Task<ConcertInfoDTO> GetConcertById(int id);
}