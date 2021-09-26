using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "This field is required!")]
        [MaxLength(30, ErrorMessage = "Max length of username field is 30!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [MaxLength(50, ErrorMessage = "Max length of password field is 50!")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string Uncorrectdetails { get; set; }
    }
}
