using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class EditTaskVM
    {
        public string TaskID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [MinLength(2, ErrorMessage = "Min length of task name field is 2!")]
        [MaxLength(70, ErrorMessage = "Max length of task name field is 70!")]
        public string TaskName { get; set; }

        [MaxLength(230, ErrorMessage = "Max length of description field is 230!")]
        public string Description { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "EndDate,StartTimeOnce,EndTimeOnce")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date, ErrorMessage = "Please input correct type of data for this field!")]
        public DateTime StartDate { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndTimeOnce")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Date, ErrorMessage = "Please input correct type of data for this field!")]
        public DateTime EndDate { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce")]
        [DataType(DataType.Time, ErrorMessage = "Please input correct type of data for this field!")]
        [Required(ErrorMessage = "This field is required!")]
        public DateTime StartTimeOnce { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndDate")]
        [DataType(DataType.Time, ErrorMessage = "Please input correct type of data for this field!")]
        [Required(ErrorMessage = "This field is required!")]
        public DateTime EndTimeOnce { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTimeCustom { get; set; }
        [DataType(DataType.Time)]
        public DateTime EndTimeCustom { get; set; }

        public string M { get; set; }
        public string T { get; set; }
        public string W { get; set; }
        public string Th { get; set; }
        public string F { get; set; }
        public string Sa { get; set; }
        public string S { get; set; }

        [Remote(action: "TimeVal", controller: "Task")]
        [Required(ErrorMessage = "This field is required!")]
        [Range(0, 500, ErrorMessage = "This field can't be negative and maximum value is 500!")]
        public int Hours { get; set; }

        [Remote(action: "TimeVal", controller: "Task")]
        [Required(ErrorMessage = "This field is required!")]
        [Range(0, 500, ErrorMessage = "This field can't be negative and maximum value is 500!")]
        public int Minutes { get; set; }
    }
}
