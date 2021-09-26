using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Models
{
    public class TaskDB
    {
        [Key]
        public int ID { get; set; }
        public string Key { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastNotification { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NotificationEvery { get; set; }
        public string Priority { get; set; }
        public int NumberOfNotifications { get; set; }
        public string SoundName { get; set; }
        public string Days { get; set; }
        public string CreatedFor { get; set; }
        public string Stats { get; set; }
        public double Successufull { get; set; }
        public bool AcceptedNotification { get; set; }
        public int NumberOfFinished { get; set; }
        public bool Rated { get; set; }
        public DateTime LastTimeFinished { get; set; }
    }
}
