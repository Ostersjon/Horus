using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castle.Models;

public class Productions
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
	[Required]
	[MinLength(3, ErrorMessage = "must be more than 3 chars")]
	[MaxLength(25, ErrorMessage = "must be less than 25 chars")]
	public string name { get; set; }
    public string photo { get; set; }
    public List<Media> medias { get; set; }  
}
