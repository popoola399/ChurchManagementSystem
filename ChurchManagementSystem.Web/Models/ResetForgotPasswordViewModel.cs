
using System.ComponentModel.DataAnnotations;

namespace ChurchManagementSystem.Web.Models
{
    public class ResetForgotPasswordViewModel
    {
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string Confirm { get; set; }

    }
}