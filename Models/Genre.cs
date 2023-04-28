using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castle.Models;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    public string Photo { get; set; }
    public List<MediaGenres> MediaGenres { get; set; }
}
