using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castle.Models;

public class Seasons
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public Media Media { get; set; } = null!;
    public int Mediaid { get; set; }
}
