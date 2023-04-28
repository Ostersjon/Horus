using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Castle.Models;

namespace Castle.Dtos;

public class ProductionsDTO
{
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(25, ErrorMessage = "must be less than 25 chars")]
    public string name { get; set; }
    public IFormFile photo { get; set; }
}
