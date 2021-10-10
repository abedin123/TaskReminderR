using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class AlarmsDaysCheckBoxVM: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            NewAlarmVM obj = (NewAlarmVM)validationContext.ObjectInstance;
            if (obj.M != "false" || obj.T != "false" || obj.W != "false" || obj.Th != "false" || obj.F != "false" || obj.Sa != "false" || obj.S != "false")
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("9");
        }
    }
}
