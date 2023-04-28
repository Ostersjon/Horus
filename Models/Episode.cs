using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castle.Models;

public class Episode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string name { get; set; }
    public Seasons seasons { get; set; } = null!;
    public int seasonsid { get; set; }
    public string Thumbnail { get; set; }
    public string Video { get; set; }

}