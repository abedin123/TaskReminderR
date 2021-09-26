using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class TaskNotificationVM
    {
        public string Key { get; set; }
        public string TaskName { get; set; }
        public DateTime LastNotification { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NotificationEvery { get; set; }
        public string Priority { get; set; }
        public string SoundName { get; set; }
        public string Days { get; set; }
        public string CreatedFor { get; set; }
        public string Stats { get; set; }
        public bool Rated { get; set; }
        public bool FinishNotificated { get; set; }
        public bool AcceptedNotification { get; set; }
    }
}
