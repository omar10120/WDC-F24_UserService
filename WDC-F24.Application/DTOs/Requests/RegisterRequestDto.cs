using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDC_F24.Application.DTOs.Requests
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username name is required.")]
        [MaxLength(256, ErrorMessage = "Username name cannot exceed 256 characters.")]
        //[RegularExpression(@"^[\p{L}\p{N}\s'-]+$", ErrorMessage = "User name can only contain letters.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Passwords must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Passwords must have at least one uppercase letter ('A'-'Z'), at least one digit ('0'-'9'), be at least 8 characters long, and may only include @, $, !, %, *, ?, & special characters.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string Phone { get; set; }

    }
}
