using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.Models
{
    public class Alarms
    {
        [Key]
        public int ID { get; set; }
        public string Key { get; set; }
        public string AlarmName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime RingingTime { get; set; }
        public DateTime LastSnooze { get; set; }
        public DateTime LastRinging { get; set; }
        public int RingDuration { get; set; }
        public int SnoozeDuration { get; set; }
        public string SoundName { get; set; }
        public bool Active { get; set; }
        public string Days { get; set; }
        public string CreatedFor { get; set; }
    }
}
