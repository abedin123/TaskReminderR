using GenerateSuccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class TempDataObject
    {
        public List<Alarms> AlarmList { get; set; }
        public List<TaskDB> TaskList { get; set; }
    }
}
