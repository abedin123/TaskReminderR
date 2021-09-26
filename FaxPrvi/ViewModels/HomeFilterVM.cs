using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateSuccess.ViewModels
{
    public class HomeFilterVM
    {
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string AlarmStatus { get; set; }
        public string M { get; set; }
        public string T { get; set; }
        public string W { get; set; }
        public string Th { get; set; }
        public string F { get; set; }
        public string Sa { get; set; }
        public string S { get; set; }
    }
}
