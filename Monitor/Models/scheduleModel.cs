using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorTrackerForm.Models
{
  public  class scheduleModel
    {
        public string UserName { get; set; }
        public DateTime? HourFrom { get; set; }
        public DateTime? HourUntil { get; set; }

    }
}
