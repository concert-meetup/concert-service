using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace concert.API.Models;

public class Concert
{
    [Key]
    [Required]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [Column("concert_date")]
    public DateTime ConcertDate { get; set; }

    [Required]
    [Column("artist")]
    [StringLength(maximumLength: 45, MinimumLength = 1)]
    public string Artist { get; set; } = string.Empty;
    
    [Required]
    [Column("image_url")]
    [StringLength(maximumLength:100, MinimumLength = 2)]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Required]
    [Column("venue_id")]
    public int VenueId { get; set; }

    public Venue Venue { get; set; }
    
    [Required]
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Required]
    [Column("modified")]
    public DateTime Modified { get; set; }
}