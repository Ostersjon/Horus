using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Castle.Models;

namespace Castle.Dtos;

public class EpisodeDTO
{
    public int seasonsid { get; set; }
    public string name { get; set; }
    public IFormFile Thumbnail { get; set; } = null!;
    public string Video { get; set; } = string.Empty;
}
