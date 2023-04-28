using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castle.Models;

public class Media
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(100, ErrorMessage = "must be less than 25 chars")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(500, ErrorMessage = "must be less than 500 chars")]
    public string about { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
	[Required]
	[MaxLength(25, ErrorMessage = "must be less than 25 chars")]
	public string Language { get; set; } = string.Empty;
	public string Discriminator { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty;

	[Required]
	[MinLength(3, ErrorMessage = "must be more than 3 chars")]
	[MaxLength(35, ErrorMessage = "must be less than 35 chars")]
	public string MovieCountry { get; set; } = string.Empty;
    [MaxLength(4)]
    [MinLength(4)]
    public string Year { get; set; } = string.Empty;
    public List<Category> Categories { get; set; } = null!;
    //public List<Rating> Ratings { get; set; } = null!;
    //public List<WishList> WishLists { get; set; } = null!;
    public Productions productions { get; set; } = null!;
    public List<MediaGenres> MediaGenres { get; set; }
    public List<Seasons> seasons { get; set; } = null!;

    public int productionsid { get; set; }
    [NotMapped]
    public Category Category { get; set; } 
    public int Categoryid { get; set; } 
}
