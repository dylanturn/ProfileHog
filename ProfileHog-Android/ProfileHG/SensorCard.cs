using System;
using Android.Views;
using Android.Widget;

namespace ProfileHG
{
	public class SensorCard
	{
		public Sensor CardSensor;
		public TextView CardResource;

		public SensorCard (TextView newCardID, Sensor newSensor)
		{
			CardSensor = newSensor;
			CardResource = newCardID;
		}
	}
}

