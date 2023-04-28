using Microsoft.EntityFrameworkCore;

namespace Castle.Models;
[PrimaryKey(nameof(Mediaid),nameof(Categoryid))]
public class MediaCategories
{
    public Media Media { get; set; } = null!;
    public int Mediaid { get; set; }
    public Category Category { get; set; } = null!;
    public int Categoryid { get; set; }
}
