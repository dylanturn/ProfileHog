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
		List<SensorValue> SensorHistory = new List<SensorValue>();

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
			Voltage
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

		public Sensor (string Name, Types Type){
			SensorName = Name;
			SensorType = Type;
		}

		public Sensor (string Name, Types Type, int value){
			SensorName = Name;
			SensorType = Type;
			this.setCurrentValue (value);
		}

		public void setCurrentValue( int value){
			SensorValue newItem = new SensorValue (value);
			CurrentValue = newItem;
			SensorHistory.Add (newItem);
			//setHighestValue ();
			//calculateAverageValue ();
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
			int valueTotal = 0;
			for (int i = 0; i < SensorHistory.Count; i++) {
				valueTotal = valueTotal + SensorHistory [i].getValue ();
			}
			AverageValue.setValue ((valueTotal / SensorHistory.Count));
		}
	}
}

