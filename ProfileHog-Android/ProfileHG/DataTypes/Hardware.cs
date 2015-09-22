using System;
using System.Collections.Generic;

namespace ProfileHG
{
	public class Hardware
	{
		public string HardwareName { get; private set; }
		public Types HardwareType { get; private set; }
		public string HardwareGenericType;
		public Dictionary<Hardware.Types, string>  HardwareGenericTypes { get; private set; }
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
			HardwareGenericTypes = new Dictionary<Hardware.Types, string> ();
			SetupHardwareGenericDict ();
		}

		public Hardware (string thisName, Types thisType, Hardware thisParent)
		{
			HardwareName = thisName;
			HardwareType = thisType;
			ParentHardware = thisParent;
			SensorList = new List<Sensor> ();
			SubHardwareList = new List<Hardware> ();
			HardwareGenericTypes = new Dictionary<Hardware.Types, string> ();
			SetupHardwareGenericDict ();
			HardwareGenericType = HardwareGenericTypes [thisType];
		}

		public void AddSubHardware(Hardware SubHardware){
			SubHardwareList.Add (SubHardware);
		}

		public void AddSensor(Sensor Sensor){
			SensorList.Add (Sensor);
		}
		private void SetupHardwareGenericDict(){
			HardwareGenericTypes.Add (Types.CPU, "CPU");
			HardwareGenericTypes.Add (Types.DISK, "DISK");
			HardwareGenericTypes.Add (Types.GpuAti, "GPU");
			HardwareGenericTypes.Add (Types.GpuNvidia, "GPU");
			HardwareGenericTypes.Add (Types.HDD, "DISK");
			HardwareGenericTypes.Add (Types.Mainboard, "MOBO");
			HardwareGenericTypes.Add (Types.RAM, "RAM");
			HardwareGenericTypes.Add (Types.SuperIO, "MISC");
			HardwareGenericTypes.Add (Types.TBalancer, "MISC");
		}
	}
}

