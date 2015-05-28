using System;

namespace ProfileHG
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

		public ProcessInformation ()
		{

		}
	}
}

