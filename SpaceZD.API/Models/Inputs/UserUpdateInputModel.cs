using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class UserUpdateInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}