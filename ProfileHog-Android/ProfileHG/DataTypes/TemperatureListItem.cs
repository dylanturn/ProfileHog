using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ProfileHG
{
	public class TemperatureListItem
	{

		public string parentName;
		public TextView sensorName;
		public TextView sensorValue;
		public Sensor thisSensor;

		public TemperatureListItem ()
		{
		}

		public TemperatureListItem (string newParent, Sensor newSensor, Context sensorContext){

			parentName = newParent;
			thisSensor = newSensor;

			sensorName = new TextView (sensorContext);
			sensorValue = new TextView (sensorContext);

			sensorName.Text = thisSensor.SensorName;
			sensorValue.Text = thisSensor.CurrentValue.getValue ().ToString ();

		}

		public void Update(Sensor updateSensor){
			sensorValue.Text = updateSensor.CurrentValue.getValue ().ToString ();
		}


	}
}

