using GenerateSuccess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class MainHomePreviewVM
    {
        public Alarms Alarm { get; set; }
        public TaskDB Task { get; set; }

        public List<TaskPreviewVM> TaskList { get; set; }
        public List<AlarmPreviewVM> AlarmList { get; set; }

        public string Success { get; set; }

        [MaxLength(70, ErrorMessage = "15")]
        [DataType(DataType.Text, ErrorMessage = "13")]
        public string TaskName { get; set; }

        [Remote(action: "TaskStatus", controller: "Task")]
        public string TaskStatus { get; set; }

        [DataType(DataType.Date, ErrorMessage = "13")]
        public DateTime From { get; set; }

        [DataType(DataType.Date, ErrorMessage = "13")]
        public DateTime To { get; set; }

        
        public string AlarmStatus { get; set; }

        public string M { get; set; }
        public string T { get; set; }
        public string W { get; set; }
        public string Th { get; set; }
        public string F { get; set; }
        public string Sa { get; set; }
        public string S { get; set; }

        public MainNotificationVM MainNotification { get; set; }
    }
}
