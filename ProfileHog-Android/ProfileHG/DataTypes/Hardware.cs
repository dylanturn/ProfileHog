using System;
using System.Collections.Generic;

namespace ProfileHG
{
	public class Hardware
	{
		public string HardwareName { get; private set; }
		public Types HardwareType { get; private set; }
		public Hardware ParentHardware { get; private set; }
		public List<Hardware> SubHardwareList { get; private set; }
		public List<Sensor> SensorList { get; private set; }

		public enum Types {
			GpuAti,
			GpuNvidia,
			CPU,
			RAM,
			DISK,
			HDD,
			Mainboard,
			SuperIO,
			TBalancer
		}

		public static Hardware.Types HardwareTypeFromString(string StringType){
			foreach (Hardware.Types type in Enum.GetValues(typeof(Hardware.Types))) {
				if (type.ToString() == StringType) {
					return type;
				}
			}
			Exception e = new Exception ("Type " + StringType + " not found");
			throw e;
		}

		public Hardware (){
			SensorList = new List<Sensor> ();
			SubHardwareList = new List<Hardware> ();
		}

		public Hardware (string thisName, Types thisType, Hardware thisParent)
		{
			HardwareName = thisName;
			HardwareType = thisType;
			ParentHardware = thisParent;
			SensorList = new List<Sensor> ();
			SubHardwareList = new List<Hardware> ();
		}

		public void AddSubHardware(Hardware SubHardware){
			SubHardwareList.Add (SubHardware);
		}

		public void AddSensor(Sensor Sensor){
			SensorList.Add (Sensor);
		}
	}
}

