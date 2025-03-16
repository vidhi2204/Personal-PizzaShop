using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set;}

}
