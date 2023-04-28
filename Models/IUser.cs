using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Castle.Models;

namespace Castle.Areas.Identity.Data;

public class IUser : IdentityUser
{
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(15, ErrorMessage = "must be less than 15 chars")]
    public string firstname { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(15, ErrorMessage = "must be less than 15 chars")]
    public string lastname { get; set; } = string.Empty;
    public string photo { get; set; } = "Default.png";
    [DataType(DataType.EmailAddress)]
    public List<Rating> Ratings { get; set; } = null!;
    public List<WishList> WishLists { get; set; } = null!;
}

