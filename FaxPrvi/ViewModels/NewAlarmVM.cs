using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class NewAlarmVM
    {
        public string User { get; set; }

        public string Key { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [MinLength(3, ErrorMessage = "Min length of alarm name field is 3!")]
        [MaxLength(70, ErrorMessage = "Max length of task alarm field is 70!")]
        [DataType(DataType.Text, ErrorMessage = "Please enter valid data in this field!")]
        public string AlarmName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Time, ErrorMessage = "Please enter valid data in this field!")]
        public DateTime RingingTime { get; set; }

        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string M { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string T { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string W { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string Th { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string F { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string Sa { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string S { get; set; }

        [Remote(action: "CreatedForVal", controller: "Alarm")]
        [Required(ErrorMessage = "This field is required!")]
        public string CreatedFor { get; set; }

        [Remote(action: "RingingDurVal", controller: "Alarm")]
        [Required(ErrorMessage = "This field is required!")]
        public string RingingDuration { get; set; }

        [Remote(action: "SnoozedurVal", controller: "Alarm")]
        [Required(ErrorMessage = "This field is required!")]
        public string SnoozeDuration { get; set; }

        [Remote(action: "SoundVal", controller: "Alarm")]
        [Required(ErrorMessage = "This field is required!")]
        public string Sound { get; set; }

        [Remote(action: "ActivityVal", controller: "Alarm")]
        [Required(ErrorMessage = "This field is required!")]
        public string Activity { get; set; }
    }
}
