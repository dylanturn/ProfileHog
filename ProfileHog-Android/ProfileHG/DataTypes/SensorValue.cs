using System;

namespace ProfileHG
{
	public class SensorValue
	{
		int value;
		DateTime dateStamp;

		public SensorValue(){
		}

		public SensorValue(int newValue){
			value = newValue;
			dateStamp = DateTime.Now;
		}

		public void setValue(int newValue){
			value = newValue;
			dateStamp = DateTime.Now;
		}

		public int getValue(){
			return value;
		}

		public DateTime getDateTime(){
			return dateStamp;
		}
	}
}

