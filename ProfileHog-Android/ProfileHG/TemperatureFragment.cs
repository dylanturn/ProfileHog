using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
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
	public class TemperatureFragment : Fragment
	{

		DataCollection dataCollection;
		LinearLayout TempDetailsParent;

		List<TemperatureListItem> TemperatureItems = new List<TemperatureListItem>();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			return inflater.Inflate(Resource.Layout.TemperatureLayout, container,false);
		}

		public override void OnActivityCreated(Bundle savedInstanceState){
			base.OnActivityCreated (savedInstanceState);

			CreateTables ();
		}

		public TemperatureFragment (DataCollection collection)
		{
			dataCollection = collection;
		}

		//Just some stuff to create the table.
		private void CreateTables(){
			ScrollView TempScrollParent = this.Activity.FindViewById <ScrollView> (Resource.Id.TemperatureScroll);
			TempDetailsParent = this.Activity.FindViewById<LinearLayout> (Resource.Id.TemperatureDetails);

			Task UIUpdateTask = new Task( () => UpdateTables() ,TaskCreationOptions.LongRunning);
			UIUpdateTask.Start();
		}

		//Finds the temp list item that has the parents name
		public TemperatureListItem FindItemByParent (string Parent){
			foreach (TemperatureListItem thisTempItem in TemperatureItems) {
				if (thisTempItem.parentName == Parent) {
					return thisTempItem;
				}
			}
			return null;
		}

		//Gets the number of temperature sensors a hardware object holds
		public int GetTempSensorCount(Hardware thisHardware){
			int tempSensCount = 0;
			foreach (Sensor thisSensor in thisHardware.SensorList) {
				if (thisSensor.SensorType == Sensor.Types.Temperature) {
					tempSensCount++;
				}
			}
			return tempSensCount;
		}

		//Updates the table of temp values.
		private void UpdateTables(){
			while (true) {
				try{
					
				foreach (Hardware thisHardware in dataCollection.HardwareList) {

					if(GetTempSensorCount(thisHardware) > 0){
							
						LinearLayout ItemLayout = new LinearLayout (TempDetailsParent.Context);
						ItemLayout.Orientation = Orientation.Vertical;

						TextView TempTitleText = new TextView (TempDetailsParent.Context);
						TempTitleText.Text = thisHardware.HardwareName;
						ItemLayout.AddView (TempTitleText);

						TableLayout ItemTableLayout = new TableLayout (TempDetailsParent.Context);
						ItemLayout.AddView (ItemTableLayout);

						foreach (Sensor thisSensor in thisHardware.SensorList) {
							if (thisSensor.SensorType == Sensor.Types.Temperature) {
								TemperatureListItem foundListItem = FindItemByParent (thisHardware.HardwareName);
								if (foundListItem != null) {
										
									if(this.Activity != null){
										this.Activity.RunOnUiThread (() => foundListItem.Update(thisSensor));
									}

								} else {
									
									TableRow ItemRow = new TableRow (TempDetailsParent.Context);
									TemperatureListItem newItem = new TemperatureListItem (thisHardware.HardwareName, thisSensor, this.Activity);

									ItemRow.AddView (newItem.sensorName);
									ItemRow.AddView (newItem.sensorValue);

									TemperatureItems.Add (newItem);
									ItemTableLayout.AddView (ItemRow);
									
									if(this.Activity != null){
										this.Activity.RunOnUiThread (() => TempDetailsParent.AddView (ItemLayout));
									}
								}
							}
						}
					}
				}
				
				//Sleeps this thread because theres no need to update faster that we are getting data.
				Thread.Sleep (500);

				} catch (Exception e){ Console.WriteLine (e.Message); }
			}
		
		}
		public override void OnDestroy ()
		{
			base.OnDestroy ();
		}
	}
}

