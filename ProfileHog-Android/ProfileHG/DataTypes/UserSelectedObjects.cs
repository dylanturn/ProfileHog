using System;
using System.Collections.Generic;

/*

This is used to control what the user wants to track in the details view.

*/
namespace ProfileHG
{
	public class UserSelectedObjects
	{

		public Dictionary<Hardware, Sensor.Types> SelectionSensors  { get; private set; }

		public UserSelectedObjects ()
		{
			SelectionSensors = new Dictionary<Hardware, Sensor.Types>();
		}

		public void AddSensor(Hardware thisHardware, Sensor.Types thisType){
			SelectionSensors.Add(thisHardware, thisType);

		}
	}
}

