using Castle.Models;

namespace Castle.Dtos;

public class HomeItemsDTO
{
    public Category Category { get; set; }
    public List<Media> Media { get; set; }
}
