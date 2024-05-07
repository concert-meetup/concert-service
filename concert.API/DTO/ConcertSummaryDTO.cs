namespace concert.API.DTO;

public class ConcertSummaryDTO
{
    public int Id { get; set; }
    public string Artist { get; set; } = string.Empty;
    public DateTime ConcertDate { get; set; }
    public VenueSummaryDTO VenueSummary { get; set; }
}