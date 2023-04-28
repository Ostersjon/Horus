using System.ComponentModel.DataAnnotations;

namespace Castle.Dtos;

public class MovieDTO
{
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(100, ErrorMessage = "must be less than 25 chars")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(500, ErrorMessage = "must be less than 500 chars")]
    public string about { get; set; } = string.Empty;
    public IFormFile Poster { get; set; } = null!;
    public IFormFile Cover { get; set; } = null!;
    [Required]
    [MaxLength(25, ErrorMessage = "must be less than 25 chars")]
    public string Language { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(35, ErrorMessage = "must be less than 35 chars")]

    public string MovieCountry { get; set; } = string.Empty;
    public string Discriminator { get; set; } = string.Empty;
    [MaxLength(4)]
    [MinLength(4)]
    public string Year { get; set; } = string.Empty;
    public int productionsid { get; set; }
    public int Categoryid { get; set; }
    public string Video { get; set; } = string.Empty;

}
