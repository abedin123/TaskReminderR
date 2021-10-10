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

        [Required(ErrorMessage = "Required")]
        [MinLength(3, ErrorMessage = "10")]
        [MaxLength(70, ErrorMessage = "11")]
        public string TaskName { get; set; }

        [MaxLength(230, ErrorMessage = "12")]
        public string Description { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "EndDate,StartTimeOnce,EndTimeOnce")]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date, ErrorMessage = "13")]
        public DateTime StartDate { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndTimeOnce")]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date, ErrorMessage = "13")]
        public DateTime EndDate { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,EndDate,EndTimeOnce")]
        [DataType(DataType.Time, ErrorMessage = "13")]
        [Required(ErrorMessage = "Required")]
        public DateTime StartTimeOnce { get; set; }

        [Remote(action: "DateVal", controller: "Task", AdditionalFields = "StartDate,StartTimeOnce,EndDate")]
        [DataType(DataType.Time, ErrorMessage = "13")]
        [Required(ErrorMessage = "Required")]
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
        [Required(ErrorMessage = "Required")]
        [Range(0, 500, ErrorMessage = "14")]
        public int Hours { get; set; }

        [Remote(action: "TimeVal", controller: "Task")]
        [Required(ErrorMessage = "Required")]
        [Range(0, 500, ErrorMessage = "14")]
        public int Minutes { get; set; }
    }
}
