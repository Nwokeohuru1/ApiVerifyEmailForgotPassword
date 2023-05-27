using System.ComponentModel.DataAnnotations;

namespace ApiVerifyEmailForgotPassword.Models
{
    public class UserRegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(5, ErrorMessage ="Password is less than five(5) characters or numbers")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
