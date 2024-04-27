using System.ComponentModel.DataAnnotations;

namespace Lesson56.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email field is empty")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password field is empty")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
