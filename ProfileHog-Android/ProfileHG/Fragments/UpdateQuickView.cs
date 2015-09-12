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
		DPIScaling dpiScale;
		public Dictionary<Hardware.Types, string> QuickViewTypesCheckedDictionary  { get; private set; }

		DataCollection dataCollection;
		MainActivity ActivityWithList;
		Activity ParentActivity;

		public UpdateQuickView (DataCollection collection, Activity parentActivity, MainActivity activityWithList)
		{
			dataCollection = collection;
			ParentActivity = parentActivity;
			ActivityWithList = activityWithList;
			quickViewGroup = parentActivity.FindViewById<LinearLayout> (Resource.Id.QuickViewGroup);
			CreateDictionary ();
			dpiScale = new DPIScaling (parentActivity);
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

							TextView existingCard = null;
							foreach (SensorCard thisCard in ActivityWithList.quickViewCards) {
								if (thisCard.CardSensor == thisSensor) {
									existingCard = (TextView) thisCard.CardResource;
								}
							}

							//View QuickViewItemParent = inflator.Inflate (Resource.Layout.QuickViewItemTemplate,null, false);
							if (QuickViewTypesCheckedDictionary.ContainsValue (thisSensor.SensorName) && (existingCard == null)) {
								RelativeLayout QuickViewItem = new RelativeLayout (ParentActivity);

								LinearLayout.LayoutParams QuickViewLayout = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
								QuickViewLayout.Width = dpiScale.GetDPI (75);
								QuickViewLayout.Height = LinearLayout.LayoutParams.MatchParent;
								QuickViewLayout.RightMargin = dpiScale.GetDPI (4);
								QuickViewLayout.LeftMargin = dpiScale.GetDPI (4);
								QuickViewItem.Elevation = 10;
								QuickViewItem.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#ffffff"));
								QuickViewItem.LayoutParameters = QuickViewLayout;

								LinearLayout QuickViewColor = new LinearLayout (ParentActivity);
								QuickViewColor.Id = View.GenerateViewId();
								TextView QuickViewTitle = new TextView (ParentActivity);
								QuickViewTitle.Id = View.GenerateViewId();
								TextView QuickViewValue = new TextView (ParentActivity);
								QuickViewValue.Id = View.GenerateViewId();
								TextView QuickViewLabel = new TextView (ParentActivity);
								QuickViewLabel.Id = View.GenerateViewId();

								RelativeLayout.LayoutParams QuickViewColorLayout = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
								QuickViewColorLayout.Width = LinearLayout.LayoutParams.MatchParent;
								QuickViewColorLayout.Height = dpiScale.GetDPI (10);
								QuickViewColorLayout.AddRule(LayoutRules.AlignParentTop);
								QuickViewColorLayout.AddRule(LayoutRules.CenterHorizontal);
								QuickViewColor.LayoutParameters = QuickViewColorLayout;
								QuickViewColor.SetBackgroundColor (Android.Graphics.Color.ParseColor (ObjectColorDictionary.dictionary [thisHardware.HardwareType]));

								RelativeLayout.LayoutParams QuickViewTitleLayout = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
								QuickViewTitleLayout.Width = RelativeLayout.LayoutParams.WrapContent;
								QuickViewTitleLayout.Height = RelativeLayout.LayoutParams.WrapContent;
								QuickViewTitleLayout.SetMargins (0, 7, 0, 0);
								QuickViewTitleLayout.AddRule (LayoutRules.Below, QuickViewColor.Id);
								QuickViewTitleLayout.AddRule(LayoutRules.CenterHorizontal);
								QuickViewTitle.SetTextSize (ComplexUnitType.Dip, 18);
								QuickViewTitle.Text = thisSensor.SensorParent.HardwareGenericType.ToString ();
								QuickViewTitle.SetTextColor (Android.Graphics.Color.ParseColor ("#ff3b3b3b"));
								QuickViewTitle.LayoutParameters = QuickViewTitleLayout;

								RelativeLayout.LayoutParams QuickViewValueLayout = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
								QuickViewValueLayout.Width = RelativeLayout.LayoutParams.WrapContent;
								QuickViewValueLayout.Height = RelativeLayout.LayoutParams.WrapContent;
								QuickViewValueLayout.AddRule(LayoutRules.CenterHorizontal);
								QuickViewValueLayout.SetMargins (0, 2, 0, 0);
								QuickViewValueLayout.AddRule (LayoutRules.Below, QuickViewTitle.Id);
								QuickViewValue.SetTextSize (ComplexUnitType.Dip, 14);
								QuickViewValue.Text = thisSensor.CurrentValue.getValue ().ToString (thisSensor.SensorValueFormat) + " (" + thisSensor.SensorUnitType + ")";
								QuickViewValue.SetTextColor (Android.Graphics.Color.ParseColor ("#ff3b3b3b"));
								QuickViewValue.LayoutParameters = QuickViewValueLayout;
							
								RelativeLayout.LayoutParams QuickViewLabelLayout = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
								QuickViewLabelLayout.Width = RelativeLayout.LayoutParams.WrapContent;
								QuickViewLabelLayout.Height = RelativeLayout.LayoutParams.WrapContent;
								QuickViewLabelLayout.AddRule (LayoutRules.AlignParentBottom);
								QuickViewLabelLayout.AddRule (LayoutRules.CenterHorizontal);
								QuickViewLabel.SetTextSize (ComplexUnitType.Dip, 10);
								QuickViewLabel.Text = thisSensor.SensorType.ToString ();
								QuickViewLabel.SetTextColor (Android.Graphics.Color.ParseColor ("#ff3b3b3b"));
								QuickViewLabel.LayoutParameters = QuickViewLabelLayout;

								QuickViewItem.AddView (QuickViewColor);
								QuickViewItem.AddView (QuickViewTitle);
								QuickViewItem.AddView (QuickViewValue);
								QuickViewItem.AddView (QuickViewLabel);

								SensorCard newSensor = new SensorCard (QuickViewValue, thisSensor);
								ActivityWithList.quickViewCards.Add (newSensor);
								LinearLayout quickViewGroup = (LinearLayout)ParentActivity.FindViewById (Resource.Id.QuickViewGroup);
								ParentActivity.RunOnUiThread (() => quickViewGroup.AddView (QuickViewItem));
							} 
							if(existingCard != null) {
									ParentActivity.RunOnUiThread (() => existingCard.Text = thisSensor.CurrentValue.getValue().ToString(thisSensor.SensorValueFormat) + " (" + thisSensor.SensorUnitType + ")");
							}
						}
					}
				}

				Thread.Sleep (500);
			}
		}
	}
}

