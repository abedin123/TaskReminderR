using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        [Required (ErrorMessage="Required")]
        [MinLength(4, ErrorMessage = "1")]
        [MaxLength(30, ErrorMessage = "2")]
        [DataType(DataType.Text, ErrorMessage = "8")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(5, ErrorMessage = "3")]
        [MaxLength(50, ErrorMessage = "4")]
        [DataType(DataType.Text, ErrorMessage = "8")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Compare("Password", ErrorMessage = "5")]
        [MaxLength(50, ErrorMessage = "4")]
        [DataType(DataType.Text, ErrorMessage = "8")]
        public string ConfirmPassword { get; set; }
    }
}
