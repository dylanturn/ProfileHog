using System;

namespace ProfileHG
{
	public class SensorValue
	{
		double value;
		DateTime dateStamp;

		public SensorValue(){
		}

		public SensorValue(double newValue){
			value = newValue;
			dateStamp = DateTime.Now;
		}

		public void setValue(double newValue){
			value = newValue;
			dateStamp = DateTime.Now;
		}

		public double getValue(){
			return value;
		}

		public DateTime getDateTime(){
			return dateStamp;
		}
	}
}

