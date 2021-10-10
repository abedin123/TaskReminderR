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
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "2")]
        [DataType(DataType.Text, ErrorMessage = "8")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "4")]
        [DataType(DataType.Text, ErrorMessage = "8")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Uncorrectdetails { get; set; }
    }
}
