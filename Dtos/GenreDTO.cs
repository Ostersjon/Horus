using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Castle.Models;

namespace Castle.Dtos;

public class GenreDTO
{
    public string Name { get; set; }
    public IFormFile Photo { get; set; }
}
