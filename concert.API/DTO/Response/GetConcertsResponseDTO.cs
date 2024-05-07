namespace concert.API.DTO.Response;

public class GetConcertsResponseDTO
{
    public IEnumerable<ConcertSummaryDTO> Concerts { get; set; } = [];
}