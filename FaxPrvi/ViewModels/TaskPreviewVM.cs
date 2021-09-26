using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class TaskPreviewVM
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public string TaskName { get; set; }
        public string DateDayStarting { get; set; }
        public string TimeStarting { get; set; }
        public string Success { get; set; }
        public string CreatedFor { get; set; }
        public int NumberOfFinished { get; set; }
    }
}
