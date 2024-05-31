namespace concert.API.DTO.Request;

public class CreateConcertRequestDTO
{
    public DateTime ConcertDate { get; set; }
    public string Artist { get; set; } = string.Empty;
    public string ImageUrl { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}