using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Castle.Models;

namespace Castle.Dtos;

public class SignDTO
{

    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(15, ErrorMessage = "must be less than 15 chars")]
    public string firstname { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(15, ErrorMessage = "must be less than 15 chars")]
    public string lastname { get; set; } = string.Empty;
    [DataType(DataType.EmailAddress)]
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = null!;
}
