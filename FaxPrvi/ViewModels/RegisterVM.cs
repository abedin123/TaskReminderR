using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class RegisterVM
    {
        [Remote(action: "UserNameVal", controller: "Account")]
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(4, ErrorMessage = "Min length of username field is 4!")]
        [MaxLength(30, ErrorMessage = "Max length of username field is 30!")]
        public string UserName { get; set; }

        [Remote(action: "PasswordVal", controller: "Account")]
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(5, ErrorMessage = "Min length of password field is 5!")]
        [MaxLength(50, ErrorMessage = "Max length of password field is 50!")]
        public string Password { get; set; }


        [Remote(action: "PasswordConfirmVal", controller: "Account")]
        [Required(ErrorMessage = "This field is required!")]
        [MinLength(5, ErrorMessage = "Min length of password field is 5!")]
        [MaxLength(50, ErrorMessage = "Max length of password field is 50!")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
    }
}
