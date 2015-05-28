using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileHog_Console
{
    public class ProcessInformation
    {
        public UInt32 Id;
        public string Name;
        public bool Responding;
        public int RamUsed;
        public int CpuUsed;
        public int DiskUsed;
        public int NetworkUsed;
    }
}
