using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Castle.Models;

namespace Castle.Dtos;

public class SeasonDTO
{
    public string name { get; set; } = string.Empty;
    public IFormFile Poster { get; set; } 
    public int Mediaid { get; set; }
}
