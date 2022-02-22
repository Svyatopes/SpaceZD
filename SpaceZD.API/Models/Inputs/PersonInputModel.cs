using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class PersonInputModel
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [StringLength(100)]
    public string Patronymic { get; set; }

    [Required]
    [StringLength(100)]
    public string Passport { get; set; }
}