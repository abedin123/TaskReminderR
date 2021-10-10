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

        [Required(ErrorMessage = "Required")]
        [MinLength(3, ErrorMessage = "6")]
        [MaxLength(70, ErrorMessage = "7")]
        [DataType(DataType.Text, ErrorMessage = "8")]
        public string AlarmName { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Time, ErrorMessage = "8")]
        public DateTime RingingTime { get; set; }

        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string M { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string T { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string W { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string Th { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string F { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string Sa { get; set; }
        [AlarmsDaysCheckBoxVM(ErrorMessage = "9")]
        public string S { get; set; }

        [Remote(action: "CreatedForVal", controller: "Alarm")]
        [Required(ErrorMessage = "Required")]
        public string CreatedFor { get; set; }

        [Remote(action: "RingingDurVal", controller: "Alarm")]
        [Required(ErrorMessage = "Required")]
        public string RingingDuration { get; set; }

        [Remote(action: "SnoozedurVal", controller: "Alarm")]
        [Required(ErrorMessage = "Required")]
        public string SnoozeDuration { get; set; }

        [Remote(action: "SoundVal", controller: "Alarm")]
        [Required(ErrorMessage = "Required")]
        public string Sound { get; set; }

        [Remote(action: "ActivityVal", controller: "Alarm")]
        [Required(ErrorMessage = "Required")]
        public string Activity { get; set; }
    }
}
