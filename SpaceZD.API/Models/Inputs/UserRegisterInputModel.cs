using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class UserRegisterInputModel
{
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин должен содержать только цифры и буквы")]
    public string Login { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль должен содержать только цифры и буквы")]
    public string Password { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}