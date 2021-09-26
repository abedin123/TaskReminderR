using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class AlarmPreviewVM
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public string AlarmName { get; set; }
        public string DateDayStarting { get; set; }
        public string TimeStarting { get; set; }
        public string Days { get; set; }
        public string CreatedFor { get; set; }
    }
}
