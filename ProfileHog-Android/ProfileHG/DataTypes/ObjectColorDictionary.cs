using System;
using System.Collections.Generic;

namespace ProfileHG
{
	public static class ObjectColorDictionary
	{
		public static Dictionary<Hardware.Types, string> dictionary  { get; private set; }

		static ObjectColorDictionary ()
		{
			dictionary = new Dictionary<Hardware.Types, string>();

			dictionary.Add(Hardware.Types.CPU, "#FF4B66");
			dictionary.Add(Hardware.Types.DISK, "#FFB246");
			dictionary.Add (Hardware.Types.HDD, "#FFB246");
			dictionary.Add (Hardware.Types.GpuAti, "#00C5F9");
			dictionary.Add (Hardware.Types.GpuNvidia, "#00C5F9");
			dictionary.Add (Hardware.Types.Mainboard, "#FFFFFF"); //I need to find a color for this.
			dictionary.Add (Hardware.Types.RAM, "#00A9AC");
			dictionary.Add (Hardware.Types.SuperIO, "#FFFFFF"); //I need to find a color for this.
			dictionary.Add (Hardware.Types.TBalancer, "#FFFFFF"); //I need to find a color for this.
		}


	}
}

