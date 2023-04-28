using System.ComponentModel.DataAnnotations;
using Castle.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Castle.Models;
[PrimaryKey(nameof(IUserId),nameof(MediaId),nameof(Type))]
public class Rating
{
    public string Type { get; set; } = string.Empty;
    public byte rating { get; set; }
    [Required]
    public string IUserId { get; set; } = string.Empty;
    public IUser IUser { get; set; } = null!;
    [Required]
    public int MediaId { get; set; }
    //public Media Media { get; set; } = null!;
}
