using System.ComponentModel.DataAnnotations;

namespace Castle.Dtos;

public class UserDTO
{

    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(15, ErrorMessage = "must be less than 15 chars")]
    public string firstname { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(15, ErrorMessage = "must be less than 15 chars")]
    public string lastname { get; set; } = string.Empty;
    public string photo { get; set; } = "Default.png";
    [DataType(DataType.EmailAddress)]
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = null!;

}
