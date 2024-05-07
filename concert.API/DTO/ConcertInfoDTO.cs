namespace concert.API.DTO;

public class ConcertInfoDTO
{
    public int Id { get; set; }
    public string Artist { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime ConcertDate { get; set; }
    public VenueInfoDTO VenueInfo { get; set; }
}