using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileHog_Console
{
    public class JSONHardware
    {
        public string Name;
        public string Type;
        public List<JSONSensor> Sensors = new List<JSONSensor>();
    }
}
