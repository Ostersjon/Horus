using Microsoft.EntityFrameworkCore;

namespace Castle.Models;
[PrimaryKey(nameof(Mediaid),nameof(Genreid))]
public class MediaGenres
{
    public int Mediaid { get; set; }
    public Media Media { get; set; }
    public int Genreid { get; set; }
    public Genre Genre { get; set; }
}
