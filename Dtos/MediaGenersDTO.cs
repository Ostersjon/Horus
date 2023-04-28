using Castle.Models;

namespace Castle.Dtos;

public class MediaGenersDTO
{
    public int Mediaid { get; set; }
    public List<int> Genreid { get; set; }
}
