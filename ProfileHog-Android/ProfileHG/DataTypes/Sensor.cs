using System;
using System.Collections.Generic;

namespace ProfileHG
{
	public class Sensor
	{
		public string SensorName { get; private set; }
		public Types SensorType { get; private set; }
		public Hardware SensorParent { get; private set; }

		public SensorValue CurrentValue { get; private set; }
		public SensorValue HighestValue { get; private set; }
		public SensorValue AverageValue { get; private set; }

		public string SensorUnitType { get; private set; }
		public string SensorValueFormat { get; private set; }

		List<SensorValue> SensorHistory = new List<SensorValue>();
		public Dictionary<Sensor.Types, string> SensorValueDictionary  { get; private set; }
		public Dictionary<Sensor.Types, string> SensorFormatDictionary  { get; private set; }

		public enum Types {
			Clock,
			Control,
			Data,
			Factor,
			Fan,
			Flow,
			Level,
			Load,
			Temperature,
			Voltage,
			Generic
		}

		public static Sensor.Types SensorTypeFromString(string StringType){
			foreach (Sensor.Types type in Enum.GetValues(typeof(Sensor.Types))) {
				if (type.ToString() == StringType) {
					return type;
				}
			}
			Exception e = new Exception ("Type " + StringType + " not found");
			throw e;
		}


		public Sensor(){
		}

		public Sensor (string Name, Hardware Parent, Types Type, double value){
			SensorValueDictionary = new Dictionary<Types, string> ();
			SensorFormatDictionary = new Dictionary<Types, string> ();
			SetupValueDictionary ();
			SetupFormatDictionary ();
			SensorUnitType = SensorValueDictionary [Type];
			SensorValueFormat = SensorFormatDictionary [Type];

			SensorName = Name;
			SensorParent = Parent;
			SensorType = Type;

			HighestValue = new SensorValue ();
			AverageValue = new SensorValue ();

			this.setCurrentValue (value);
		}

		public void setCurrentValue(double value){

			SensorValue newItem;

			if ((this.SensorType == Types.Temperature) && (this.SensorParent.HardwareType == Hardware.Types.GpuNvidia)) {
				Console.WriteLine (this.SensorName + " : " + value.ToString());
			}
			//Im doing this because I prefer to show memory used in MB.
			//One day GB will preffered, but I don't think we're quire there yet.
			if ((this.SensorType == Types.Data) && (this.SensorParent.HardwareType == Hardware.Types.RAM)) {
				
				newItem = new SensorValue ((value*1024));

			} else {
				
				newItem = new SensorValue (value);

			}

			CurrentValue = newItem;
			SensorHistory.Add (newItem);
			setHighestValue ();
			calculateAverageValue ();
		}

		private void SetupValueDictionary(){
			SensorValueDictionary.Add (Types.Clock, "MHz");
			SensorValueDictionary.Add (Types.Control, "%");
			SensorValueDictionary.Add (Types.Data, "MB");
			SensorValueDictionary.Add (Types.Factor, " ");
			SensorValueDictionary.Add (Types.Fan, "RPM");
			SensorValueDictionary.Add (Types.Flow, "L/h");
			SensorValueDictionary.Add (Types.Level, "%");
			SensorValueDictionary.Add (Types.Load, "%");
			SensorValueDictionary.Add (Types.Temperature, "°C");
			SensorValueDictionary.Add (Types.Voltage, "V");
			SensorValueDictionary.Add (Types.Generic, " ");
		}

		private void SetupFormatDictionary(){
			SensorFormatDictionary.Add (Types.Clock, "0");
			SensorFormatDictionary.Add (Types.Control, "0.0");
			SensorFormatDictionary.Add (Types.Data, "0");
			SensorFormatDictionary.Add (Types.Factor, "0");
			SensorFormatDictionary.Add (Types.Fan, "0");
			SensorFormatDictionary.Add (Types.Flow, "0.0");
			SensorFormatDictionary.Add (Types.Level, "0.0");
			SensorFormatDictionary.Add (Types.Load, "0.0");
			SensorFormatDictionary.Add (Types.Temperature, "0.0");
			SensorFormatDictionary.Add (Types.Voltage, "0.0");
			SensorFormatDictionary.Add (Types.Generic, "0");
		}

		void setHighestValue(){
			int maxPosition = 0;
			for (int i = 0; i < SensorHistory.Count; i++) {
				if (SensorHistory[i].getValue() > SensorHistory[maxPosition].getValue()) {
					maxPosition = i;
				}
			}
			HighestValue.setValue (SensorHistory [maxPosition].getValue ());
		}

		void calculateAverageValue(){
			double valueTotal = 0;
			for (int i = 0; i < SensorHistory.Count; i++) {
				valueTotal = valueTotal + SensorHistory [i].getValue ();
			}
			AverageValue.setValue ((valueTotal / SensorHistory.Count));
		}
	}
}

