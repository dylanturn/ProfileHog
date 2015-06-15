using System;
using System.Net.Sockets;
using System.Net;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mono;
using Android.Webkit;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Collections;

namespace ProfileHG
{
	public class UpdateQuickView
	{
		LinearLayout quickViewGroup;
		List<RelativeLayout> quickViewGroupItems = new List<RelativeLayout>();

		public Dictionary<Hardware.Types, string> QuickViewTypesCheckedDictionary  { get; private set; }

		DataCollection dataCollection;
		Activity ParentActivity;

		public UpdateQuickView (DataCollection collection, Activity parentActivity)
		{
			dataCollection = collection;
			ParentActivity = parentActivity;

			quickViewGroup = parentActivity.FindViewById<LinearLayout> (Resource.Id.QuickViewGroup);
			CreateDictionary ();
		}

		private void CreateDictionary(){
			QuickViewTypesCheckedDictionary = new Dictionary<Hardware.Types, string> ();
			QuickViewTypesCheckedDictionary.Add (Hardware.Types.CPU, "CPU Total");
			QuickViewTypesCheckedDictionary.Add (Hardware.Types.RAM, "Used Memory");
			QuickViewTypesCheckedDictionary.Add (Hardware.Types.HDD, "DiskActive");
			QuickViewTypesCheckedDictionary.Add (Hardware.Types.GpuNvidia, "GPU Core");
			QuickViewTypesCheckedDictionary.Add (Hardware.Types.GpuAti, "GPU Core");
		}

		public void StartUIUpdate(LayoutInflater inflator){
			while (true) {
				foreach (Hardware thisHardware in dataCollection.HardwareList) {
					if (QuickViewTypesCheckedDictionary.ContainsKey (thisHardware.HardwareType)) {
						foreach (Sensor thisSensor in thisHardware.SensorList) {
							if (QuickViewTypesCheckedDictionary.ContainsValue (thisSensor.SensorName)) {
								
								View QuickViewItemParent = inflator.Inflate (Resource.Layout.QuickViewItemTemplate,null, false);
								RelativeLayout QuickViewItemTemplate = (RelativeLayout) QuickViewItemParent.FindViewById (Resource.Id.QuickViewTemplate);

								RelativeLayout QuickViewItem = new RelativeLayout (ParentActivity);
								QuickViewItem.Elevation = 10;
								QuickViewItem.SetBackgroundColor(Android.Graphics.Color.ParseColor ("#ffffff"));
								QuickViewItem.LayoutParameters = QuickViewItemTemplate.LayoutParameters;

								LinearLayout QuickViewColorTemplate = (LinearLayout) QuickViewItemTemplate.FindViewById(Resource.Id.SensorColor);

								RelativeLayout tempItem = new RelativeLayout (ParentActivity);
								tempItem = QuickViewItemTemplate;

								TextView QuickViewTitleTemplate = (TextView) tempItem.FindViewById(Resource.Id.SensorType);
								TextView QuickViewValueTemplate = (TextView) tempItem.FindViewById(Resource.Id.SensorValue);
								TextView QuickViewLabelTemplate = (TextView) tempItem.FindViewById(Resource.Id.SensorLabel);

								LinearLayout QuickViewColor = new LinearLayout (ParentActivity);
								TextView QuickViewTitle = new TextView (ParentActivity);
								TextView QuickViewValue = new TextView (ParentActivity);
								TextView QuickViewLabel = new TextView (ParentActivity);

								QuickViewColor.LayoutParameters = QuickViewColorTemplate.LayoutParameters;
								QuickViewTitle.LayoutParameters = QuickViewTitleTemplate.LayoutParameters;
								QuickViewValue.LayoutParameters = QuickViewValueTemplate.LayoutParameters;
								QuickViewLabel.LayoutParameters = QuickViewLabelTemplate.LayoutParameters;

								QuickViewColorTemplate.SetBackgroundColor (Android.Graphics.Color.ParseColor (ObjectColorDictionary.dictionary[thisHardware.HardwareType]));
								QuickViewTitleTemplate.Text = thisSensor.SensorName;
								QuickViewValueTemplate.Text = thisSensor.CurrentValue.getValue ().ToString (thisSensor.SensorValueFormat);
								QuickViewLabelTemplate.Text = thisSensor.SensorType + " (" + thisSensor.SensorUnitType + ")";

								QuickViewItem.AddView (QuickViewColor);
								QuickViewItem.AddView (QuickViewTitle);
								QuickViewItem.AddView (QuickViewValue);
								QuickViewItem.AddView (QuickViewLabel);

								LinearLayout quickViewGroup = (LinearLayout) ParentActivity.FindViewById (Resource.Id.QuickViewGroup);
								ParentActivity.RunOnUiThread (() => quickViewGroup.AddView (tempItem));
							}
						}
					}
				}

				Thread.Sleep (500);
			}
		}
	}
}

