using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Castle.Dtos;

public class CartoonDTO
{
    [System.ComponentModel.DataAnnotations.Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(25, ErrorMessage = "must be less than 25 chars")]
    public string Title { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(500, ErrorMessage = "must be less than 500 chars")]
    public string about { get; set; } = string.Empty;
    public IFormFile Poster { get; set; }
    public IFormFile Cover { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    [MaxLength(25, ErrorMessage = "must be less than 25 chars")]
    public string Language { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(35, ErrorMessage = "must be less than 35 chars")]
    public string MovieCountry { get; set; } = string.Empty;
    [MaxLength(4)]
    [MinLength(4)]
    public string Year { get; set; } = string.Empty;
    public int productionsid { get; set; }
    public int Categoryid { get; set; }
}