using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class NewTaskVM
    {
        public string User { get; set; }

        public string Key { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(3, ErrorMessage = "10")]
        [MaxLength(70, ErrorMessage = "11")]
        [DataType(DataType.Text, ErrorMessage = "13")]
        public string TaskName { get; set; }
        
        [MaxLength(230, ErrorMessage = "12")]
        [DataType(DataType.Text, ErrorMessage = "13")]
        public string Description { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "EndDate,StartTimeOnce,EndTimeOnce,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date,ErrorMessage = "13")]
        public DateTime StartDate { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [DataType(DataType.Time, ErrorMessage = "13")]
        [Required(ErrorMessage = "Required")]
        public DateTime StartTimeOnce { get; set; }

        [DataType(DataType.Time, ErrorMessage = "13")]
        [Required(ErrorMessage = "Required")]
        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce,Hours,Minutes,StartTimeOnce,EndTimeCustom,CreatedFor")]
        public DateTime StartTimeCustom { get; set; }

        [Remote(action: "EndDateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndTimeOnce,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date, ErrorMessage = "13")]
        public DateTime EndDate { get; set; }

        [Remote(action: "EndDateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndDate,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [DataType(DataType.Time, ErrorMessage = "13")]
        [Required(ErrorMessage = "Required")]
        public DateTime EndTimeOnce { get; set; }

        [DataType(DataType.Time, ErrorMessage = "13")]
        [Required(ErrorMessage = "Required")]
        [Remote(action: "EndDateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce,Hours,Minutes,StartTimeOnce,StartTimeCustom,CreatedFor")]
        public DateTime EndTimeCustom { get; set; }

        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string M { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string T { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string W { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string Th { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string F { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string Sa { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "9")]
        public string S { get; set; }

        [Remote(action: "CreatedForVal", controller: "Task")]
        [Required(ErrorMessage = "Required")]
        public string CreatedFor { get; set; }

        [Remote(action: "PriorityVal", controller: "Task")]
        [Required(ErrorMessage = "Required")]
        public string Priority { get; set; }

        [Remote(action: "SoundVal", controller: "Task")]
        [Required(ErrorMessage = "Required")]
        public string Sound { get; set; }

        [Remote(action: "TimeValHours", controller: "Task", AdditionalFields = "Minutes")]
        [Required(ErrorMessage = "Required")]
        public int Hours { get; set; }

        [Remote(action: "TimeValMinutes", controller: "Task", AdditionalFields = "Hours")]
        [Required(ErrorMessage = "Required")]
        public int Minutes { get; set; }
    }
}
