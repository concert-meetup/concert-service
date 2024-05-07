using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace concert.API.Models;

public class Venue
{
    [Key]
    [Required]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [Column("name")]
    [StringLength(maximumLength:45, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Column("city")]
    [StringLength(maximumLength:45, MinimumLength = 2)]
    public string City { get; set; } = string.Empty;

    [Required]
    [Column("country")]
    [StringLength(maximumLength: 45, MinimumLength = 2)]
    public string Country { get; set; } = string.Empty;
    
    [Required]
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Required]
    [Column("modified")]
    public DateTime Modified { get; set; }

    public ICollection<Concert> Concerts { get; set; } = new List<Concert>();
}