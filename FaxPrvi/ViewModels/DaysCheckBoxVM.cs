using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class DaysCheckBoxVM : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            NewTaskVM obj = (NewTaskVM)validationContext.ObjectInstance;
            if (obj.M != "false" || obj.T != "false" || obj.W != "false" || obj.Th != "false" || obj.F != "false" || obj.Sa != "false" || obj.S != "false")
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("9");
        }
    }
}
