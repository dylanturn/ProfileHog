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
	public class DetailsFragment : Fragment
	{
		DataCollection dataCollection;
		bool UpdateUI = true;
		Task UIUpdateTask;
		LinearLayout DetailsParent;
		List<TemperatureListItem> TemperatureItems = new List<TemperatureListItem>();
		List<string> hardwareAdded = new List<string>();

		//This is temporary, I promise
		List<Sensor.Types> sensorTypesChecked = new List<Sensor.Types>();
		List<string> sensorNamesChecked = new List<string>();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your fragment here

			sensorTypesChecked.Add (Sensor.Types.Data);
			sensorTypesChecked.Add (Sensor.Types.Load);

			sensorNamesChecked.Add ("CPU Total");
			sensorNamesChecked.Add ("Used Memory");
			sensorNamesChecked.Add ("GPU Core");
			sensorNamesChecked.Add ("GPU Memory");
			sensorNamesChecked.Add ("GPU Memory Controller");
			sensorNamesChecked.Add ("Used Space");
			sensorNamesChecked.Add ("DiskReads");
			sensorNamesChecked.Add ("DiskWrites");
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			return inflater.Inflate(Resource.Layout.DetailsLayout, container, false);
		}

		public override void OnActivityCreated(Bundle savedInstanceState){
			base.OnActivityCreated (savedInstanceState);
			CreateTables ();
		}

		public DetailsFragment (DataCollection collection)
		{
			dataCollection = collection;
		}


		//Just some stuff to create the table.
		private void CreateTables(){
			ScrollView DetailsScrollParent = this.Activity.FindViewById <ScrollView> (Resource.Id.DetailsScroll);
			DetailsParent = this.Activity.FindViewById<LinearLayout> (Resource.Id.DetailsDetails);

			UIUpdateTask = new Task( () => UpdateTables() ,TaskCreationOptions.LongRunning);
			UIUpdateTask.Start();
		}

		//Finds the temp list item that has the parents name
		public TemperatureListItem FindItemByParent (string Parent, Sensor thisSensor){
			foreach (TemperatureListItem thisTempItem in TemperatureItems) {
				if (thisTempItem.parentName == Parent && thisTempItem.sensorName.Text == thisSensor.SensorName) {
					return thisTempItem;
				}
			}
			return null;
		}

		//Gets the number of temperature sensors a hardware object holds
		public int GetDetailsSensorCount(Hardware thisHardware){
			int detailsSensCount = 0;
			foreach (Sensor thisSensor in thisHardware.SensorList) {
				if (thisSensor.SensorType != Sensor.Types.Temperature) {
					detailsSensCount++;
				}
			}
			return detailsSensCount;
		}


		//Updates the table of temp values.
		private void UpdateTables(){
			while (UpdateUI) {
				try{

					foreach (Hardware thisHardware in dataCollection.HardwareList) {

						if(GetDetailsSensorCount(thisHardware) > 0){


							DPIScaling dpiScale = new DPIScaling(this.Activity);
							TableLayout ItemTableLayout = new TableLayout (DetailsParent.Context);

							if(!hardwareAdded.Contains(thisHardware.HardwareName)){

								//Create the item entry for the hardware object.
								LinearLayout ItemLayout = new LinearLayout (DetailsParent.Context);
								ItemLayout.Orientation = Orientation.Vertical;
								ItemLayout.SetGravity(GravityFlags.Center);

								//Sets the layout parameters
								LinearLayout.LayoutParams itemLayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent);
								itemLayoutParams.LeftMargin = dpiScale.GetDPI(10);
								itemLayoutParams.RightMargin = dpiScale.GetDPI(10);
								itemLayoutParams.TopMargin = dpiScale.GetDPI(10);

								ItemLayout.LayoutParameters = itemLayoutParams;

								//Create and add the Hardware title text to the item entry.
								TextView TempTitleText = new TextView (DetailsParent.Context);
								TempTitleText.SetTextSize (ComplexUnitType.Dip, 18);
								TempTitleText.Text = thisHardware.HardwareName;
								TempTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
								ItemLayout.AddView (TempTitleText);

								LinearLayout TitleTextSeperator = new LinearLayout(this.Activity);
								TitleTextSeperator.Orientation = Orientation.Vertical;


								//Gets the color of the hardware object by looking it up in a dictionary
								ObjectColorDictionary colorDictionary = new ObjectColorDictionary();
								TitleTextSeperator.SetBackgroundColor(Android.Graphics.Color.ParseColor(colorDictionary.dictionary[thisHardware.HardwareType]));
								LinearLayout.LayoutParams TitleSeperatorParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
								TitleSeperatorParams.Height = dpiScale.GetDPI(5);
								TitleTextSeperator.LayoutParameters = TitleSeperatorParams;
								ItemLayout.AddView(TitleTextSeperator);


								//Create and add the sensor table to the item entry, right below the tile text.

								ItemLayout.AddView (ItemTableLayout);

								//create a new row for the header.
								TableRow headerRow = new TableRow (DetailsParent.Context);


								TableRow.LayoutParams sensorParams = new TableRow.LayoutParams ();
								sensorParams.Width = dpiScale.GetDPI(140);

								TableRow.LayoutParams counterParams = new TableRow.LayoutParams ();
								counterParams.LeftMargin = dpiScale.GetDPI(10);

								TextView sensorTitleText = new TextView (DetailsParent.Context);
								sensorTitleText.Text = "Sensor";
								sensorTitleText.SetTextSize (ComplexUnitType.Dip, 15);
								sensorTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
								sensorTitleText.LayoutParameters = sensorParams;

								TextView currentTitleText = new TextView (DetailsParent.Context);
								currentTitleText.Text = "Current";
								currentTitleText.SetTextSize (ComplexUnitType.Dip, 15);
								currentTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
								currentTitleText.LayoutParameters = counterParams;

								TextView highestTitleText = new TextView (DetailsParent.Context);
								highestTitleText.Text = "Highest";
								highestTitleText.SetTextSize (ComplexUnitType.Dip, 15);
								highestTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
								highestTitleText.LayoutParameters = counterParams;

								TextView averageTitleText = new TextView (DetailsParent.Context);
								averageTitleText.Text = "Average";
								averageTitleText.SetTextSize (ComplexUnitType.Dip, 15);
								averageTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
								averageTitleText.LayoutParameters = counterParams;

								headerRow.AddView(sensorTitleText);
								headerRow.AddView(currentTitleText);
								headerRow.AddView(highestTitleText);
								headerRow.AddView(averageTitleText);
								ItemTableLayout.AddView(headerRow);


								this.Activity.RunOnUiThread (() => DetailsParent.AddView (ItemLayout));
								hardwareAdded.Add(thisHardware.HardwareName);
							}

							foreach (Sensor thisSensor in thisHardware.SensorList) {
								
								if (sensorTypesChecked.Contains(thisSensor.SensorType) && sensorNamesChecked.Contains(thisSensor.SensorName)) {
									TemperatureListItem foundListItem = FindItemByParent (thisHardware.HardwareName, thisSensor);
									if (foundListItem != null) {

										if(this.Activity != null){
											
											this.Activity.RunOnUiThread (() => foundListItem.Update(thisSensor));
										}

									}

									else {

										TableRow ItemRow = new TableRow (DetailsParent.Context);
										TemperatureListItem newItem = new TemperatureListItem (thisHardware.HardwareName, thisSensor, this.Activity);

										ItemRow.AddView (newItem.sensorName);
										ItemRow.AddView (newItem.sensorValue);
										ItemRow.AddView (newItem.highestValue);
										ItemRow.AddView (newItem.averageValue);

										TemperatureItems.Add (newItem);
										this.Activity.RunOnUiThread (() => ItemTableLayout.AddView (ItemRow));
									}
								}
							}
						}
					}
				}catch {
					
				}

				//no need to update faster than we get pulling the data.
				Thread.Sleep (500);
			}
		}



		public override void OnDestroy ()
		{
			/* We need to shut down the task by letting the while loop end.
			   Once we change the UpdateUI bool to false we wait for the while loop
			   to end.  We then dispose the task and finally dispose the fragment. */

			Console.WriteLine ("Closing details fragment");
			UpdateUI = false;
			UIUpdateTask.Wait ();
			UIUpdateTask.Dispose ();
			base.OnDestroy ();
		}

	}
}

