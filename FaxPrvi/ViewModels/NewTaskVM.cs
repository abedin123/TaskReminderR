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

        [Required(ErrorMessage = "This field is required!")]
        [MinLength(3, ErrorMessage = "Min length of task name field is 3!")]
        [MaxLength(70, ErrorMessage = "Max length of task name field is 70!")]
        [DataType(DataType.Text, ErrorMessage = "Please enter valid data in this field!")]
        public string TaskName { get; set; }
        
        [MaxLength(230, ErrorMessage = "Max length of description field is 230!")]
        [DataType(DataType.Text, ErrorMessage = "Please enter valid data in this field!")]
        public string Description { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "EndDate,StartTimeOnce,EndTimeOnce,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date,ErrorMessage = "Please enter valid data in this field!")]
        public DateTime StartDate { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [DataType(DataType.Time, ErrorMessage = "Please enter valid data in this field!")]
        [Required(ErrorMessage = "This field is required!")]
        public DateTime StartTimeOnce { get; set; }

        [DataType(DataType.Time, ErrorMessage = "Please enter valid data in this field!")]
        [Required(ErrorMessage = "This field is required!")]
        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce,Hours,Minutes,StartTimeOnce,EndTimeCustom,CreatedFor")]
        public DateTime StartTimeCustom { get; set; }

        [Remote(action: "EndDateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndTimeOnce,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date, ErrorMessage = "Please enter valid data in this field!")]
        public DateTime EndDate { get; set; }

        [Remote(action: "EndDateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndDate,Hours,Minutes,StartTimeCustom,EndTimeCustom,CreatedFor")]
        [DataType(DataType.Time, ErrorMessage = "Please enter valid data in this field!")]
        [Required(ErrorMessage = "This field is required!")]
        public DateTime EndTimeOnce { get; set; }

        [DataType(DataType.Time, ErrorMessage = "Please enter valid data in this field!")]
        [Required(ErrorMessage = "This field is required!")]
        [Remote(action: "EndDateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce,Hours,Minutes,StartTimeOnce,StartTimeCustom,CreatedFor")]
        public DateTime EndTimeCustom { get; set; }

        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string M { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string T { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string W { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string Th { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string F { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string Sa { get; set; }
        [DaysCheckBoxVM(ErrorMessage = "At least one day must be marked!")]
        public string S { get; set; }

        [Remote(action: "CreatedForVal", controller: "Task")]
        [Required(ErrorMessage = "This field is required!")]
        public string CreatedFor { get; set; }

        [Remote(action: "PriorityVal", controller: "Task")]
        [Required(ErrorMessage = "This field is required!")]
        public string Priority { get; set; }

        [Remote(action: "SoundVal", controller: "Task")]
        [Required(ErrorMessage = "This field is required!")]
        public string Sound { get; set; }

        [Remote(action: "TimeValHours", controller: "Task", AdditionalFields = "Minutes")]
        [Required(ErrorMessage = "This field is required!")]
        public int Hours { get; set; }

        [Remote(action: "TimeValMinutes", controller: "Task", AdditionalFields = "Hours")]
        [Required(ErrorMessage = "This field is required!")]
        public int Minutes { get; set; }
    }
}
