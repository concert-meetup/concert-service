namespace concert.API.DTO;

public class ConcertDTO
{
    public int Id { get; set; }
    public DateTime ConcertDate { get; set; }
    public string Artist { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public VenueDTO Venue { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}