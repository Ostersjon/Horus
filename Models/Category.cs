using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castle.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    [Required]
    [MinLength(3,ErrorMessage ="must be more than 3 chars")]
    [MaxLength(25,ErrorMessage = "must be less than 25 chars")]
    public string Name { get; set; } = string.Empty;
    public List<Media> Media { get; set; } = null!;
}
