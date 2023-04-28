using Microsoft.EntityFrameworkCore;
using Castle.Areas.Identity.Data;

namespace Castle.Models;
[PrimaryKey(nameof(IuserId),nameof(MediaId),nameof(Type))]
public class WishList
{
    public string Type { get; set; } = string.Empty;
    public string IuserId { get; set; } = string.Empty;
    public IUser Iuser { get; set; } = null!;
    public int MediaId { get; set; } 
    //public Media Media { get; set; } = null!;
}
