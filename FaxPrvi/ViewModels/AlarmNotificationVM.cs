using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class AlarmNotificationVM
    {
        public string Key { get; set; }
        public string AlarmName { get; set; }
        public DateTime RingingTime { get; set; }
        public DateTime LastSnooze { get; set; }
        public DateTime LastAlarmRinging { get; set; }
        public int RingDuration { get; set; }
        public int SnoozeDuration { get; set; }
        public string SoundName { get; set; }
        public string Days { get; set; }
        public string CreatedFor { get; set; }
    }
}
