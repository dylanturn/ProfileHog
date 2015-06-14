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
		public TextView highestValue;
		public TextView averageValue;
		public Sensor thisSensor;

		public TemperatureListItem ()
		{
		}

		public TemperatureListItem (string newParent, Sensor newSensor, Context sensorContext){

			parentName = newParent;
			thisSensor = newSensor;

			sensorName = new TextView (sensorContext);
			sensorValue = new TextView (sensorContext);
			highestValue = new TextView (sensorContext);
			averageValue = new TextView (sensorContext);

			DPIScaling dpiScale = new DPIScaling(sensorContext);
			TableRow.LayoutParams counterParams = new TableRow.LayoutParams ();
			counterParams.LeftMargin = dpiScale.GetDPI(15);

			sensorName.Text = thisSensor.SensorName;
			sensorName.SetTextSize (ComplexUnitType.Dip, 15);
			sensorName.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));

			sensorValue.Text = thisSensor.CurrentValue.getValue ().ToString (thisSensor.SensorValueFormat) + " " + thisSensor.SensorUnitType;
			sensorValue.SetTextSize (ComplexUnitType.Dip, 15);
			sensorValue.Gravity = GravityFlags.Center;
			sensorValue.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
			sensorValue.LayoutParameters = counterParams;

			highestValue.Text = thisSensor.HighestValue.getValue ().ToString (thisSensor.SensorValueFormat) + " " + thisSensor.SensorUnitType;
			highestValue.SetTextSize (ComplexUnitType.Dip, 15);
			highestValue.Gravity = GravityFlags.Center;
			highestValue.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
			highestValue.LayoutParameters = counterParams;

			averageValue.Text = thisSensor.AverageValue.getValue ().ToString (thisSensor.SensorValueFormat) + " " + thisSensor.SensorUnitType;
			averageValue.SetTextSize (ComplexUnitType.Dip, 15);
			averageValue.Gravity = GravityFlags.Center;
			averageValue.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
			averageValue.LayoutParameters = counterParams;
		}

		public void Update(Sensor updateSensor){
			sensorValue.Text = updateSensor.CurrentValue.getValue ().ToString (thisSensor.SensorValueFormat) + " " + thisSensor.SensorUnitType;
			highestValue.Text = updateSensor.HighestValue.getValue ().ToString (thisSensor.SensorValueFormat) + " " + thisSensor.SensorUnitType;
			averageValue.Text = updateSensor.AverageValue.getValue ().ToString (thisSensor.SensorValueFormat) + " " + thisSensor.SensorUnitType;
		}


	}
}

