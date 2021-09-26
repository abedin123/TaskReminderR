using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class MainNotificationVM
    {
        public List<AlarmNotificationVM> AlarmList { get; set; }
        public List<TaskNotificationVM> TaskList { get; set; }
    }
}
