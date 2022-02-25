using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class UserUpdatePasswordInputModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль должен содержать только цифры и буквы")]
    public string PasswordOld { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль должен содержать только цифры и буквы")]
    public string PasswordNew { get; set; }
    
    [Required]
    [Compare("PasswordNew")]
    public string PasswordNewRepeat { get; set; }
}