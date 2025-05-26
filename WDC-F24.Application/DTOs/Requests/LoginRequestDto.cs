using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDC_F24.Application.DTOs.Requests
{
    public class LoginRequestDto
    {


        [Required(ErrorMessage = "Email or Username is required.")]
        [MaxLength(256, ErrorMessage = "Email or Username cannot exceed 256 characters.")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Passwords must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Passwords must have at least one uppercase letter ('A'-'Z'), at least one digit ('0'-'9'), be at least 8 characters long, and may only include @, $, !, %, *, ?, & special characters.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        public string Password { get; set; }


    }
}
