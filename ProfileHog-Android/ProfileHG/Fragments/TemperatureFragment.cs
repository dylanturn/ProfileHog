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
		Task UIUpdateTask;
		bool UpdateUI = true;
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

			UIUpdateTask = new Task( () => UpdateTables() ,TaskCreationOptions.LongRunning);
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
			while (UpdateUI) {
				try{
					
					foreach (Hardware thisHardware in dataCollection.HardwareList) {

						if(GetTempSensorCount(thisHardware) > 0){

							DPIScaling dpiScale = new DPIScaling(this.Activity);

							//Create the item entry for the hardware object.
							LinearLayout ItemLayout = new LinearLayout (TempDetailsParent.Context);
							ItemLayout.Orientation = Orientation.Vertical;
							ItemLayout.SetGravity(GravityFlags.Center);

							//Sets the layout parameters
							LinearLayout.LayoutParams itemLayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent);
							itemLayoutParams.LeftMargin = dpiScale.GetDPI(10);
							itemLayoutParams.RightMargin = dpiScale.GetDPI(10);
							itemLayoutParams.TopMargin = dpiScale.GetDPI(10);

							ItemLayout.LayoutParameters = itemLayoutParams;

							//Create and add the Hardware title text to the item entry.
							TextView TempTitleText = new TextView (TempDetailsParent.Context);
							TempTitleText.SetTextSize (ComplexUnitType.Dip, 18);
							TempTitleText.Text = thisHardware.HardwareName;
							TempTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#191919"));
							ItemLayout.AddView (TempTitleText);

							LinearLayout TitleTextSeperator = new LinearLayout(this.Activity);
							TitleTextSeperator.Orientation = Orientation.Vertical;


							//Gets the color of the hardware object by looking it up in a dictionary
						
							TitleTextSeperator.SetBackgroundColor(Android.Graphics.Color.ParseColor(ObjectColorDictionary.dictionary[thisHardware.HardwareType]));
							LinearLayout.LayoutParams TitleSeperatorParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
							TitleSeperatorParams.Height = dpiScale.GetDPI(5);
							TitleTextSeperator.LayoutParameters = TitleSeperatorParams;
							ItemLayout.AddView(TitleTextSeperator);


							//Create and add the sensor table to the item entry, right below the tile text.
							TableLayout ItemTableLayout = new TableLayout (TempDetailsParent.Context);
							ItemLayout.AddView (ItemTableLayout);

							//create a new row for the header.
							TableRow headerRow = new TableRow (TempDetailsParent.Context);


							TableRow.LayoutParams sensorParams = new TableRow.LayoutParams ();
							sensorParams.Width = dpiScale.GetDPI(100);

							TableRow.LayoutParams counterParams = new TableRow.LayoutParams ();
							counterParams.LeftMargin = dpiScale.GetDPI(15);

							TextView sensorTitleText = new TextView (TempDetailsParent.Context);
							sensorTitleText.Text = "Sensor";
							sensorTitleText.SetTextSize (ComplexUnitType.Dip, 16);
							sensorTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#191919"));
							sensorTitleText.LayoutParameters = sensorParams;

							TextView currentTitleText = new TextView (TempDetailsParent.Context);
							currentTitleText.Text = "Current";
							currentTitleText.SetTextSize (ComplexUnitType.Dip, 16);
							currentTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#191919"));
							currentTitleText.LayoutParameters = counterParams;

							TextView highestTitleText = new TextView (TempDetailsParent.Context);
							highestTitleText.Text = "Highest";
							highestTitleText.SetTextSize (ComplexUnitType.Dip, 16);
							highestTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#191919"));
							highestTitleText.LayoutParameters = counterParams;

							TextView averageTitleText = new TextView (TempDetailsParent.Context);
							averageTitleText.Text = "Average";
							averageTitleText.SetTextSize (ComplexUnitType.Dip, 16);
							averageTitleText.SetTextColor(Android.Graphics.Color.ParseColor("#191919"));
							averageTitleText.LayoutParameters = counterParams;

							headerRow.AddView(sensorTitleText);
							headerRow.AddView(currentTitleText);
							headerRow.AddView(highestTitleText);
							headerRow.AddView(averageTitleText);
							ItemTableLayout.AddView(headerRow);


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
										ItemRow.AddView (newItem.highestValue);
										ItemRow.AddView (newItem.averageValue);

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
			/* We need to shut down the task by letting the while loop end.
			   Once we change the UpdateUI bool to false we wait for the while loop
			   to end.  We then dispose the task and finally dispose the fragment. */
			
			Console.WriteLine ("Closing temperature fragment");
			UpdateUI = false;
			UIUpdateTask.Wait ();
			UIUpdateTask.Dispose ();
			base.OnDestroy ();
		}
	}
}

