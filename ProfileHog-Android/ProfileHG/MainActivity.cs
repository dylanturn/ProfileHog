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
	[Activity (Theme = "@android:style/Theme.DeviceDefault.NoActionBar",Label = "Profile Hog", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class MainActivity : Activity
	{
		public DataCollection dataCollection = new DataCollection ();
		//public UserSelectedObjects userSelectedSensors = new UserSelectedObjects ();

		LinearLayout activeButtonLayout;
		DPIScaling dpiScaling;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			//SetContentView (Resource.Layout.Main);
			SetContentView (Resource.Layout.ProfileHogMain);

			/*
			dataCollection.OnStartCommand (this.Intent, StartCommandFlags.Retry, 0);
			activeButtonLayout = FindViewById<LinearLayout> (Resource.Id.ActiveButtonLayout);
			dpiScaling = new DPIScaling (this);

			GraphFragment graphFragment = new GraphFragment ();
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, graphFragment).Commit ();

			Task quickViewUpdateTask = new Task( () => new UpdateQuickView (dataCollection, this).StartUIUpdate (),TaskCreationOptions.LongRunning);
			quickViewUpdateTask.Start();

			Button detailsButton = FindViewById<Button> (Resource.Id.DetailsMenuButton);
			Button graphButton = FindViewById<Button> (Resource.Id.GraphMenuButton);
			Button temperatureButton = FindViewById<Button> (Resource.Id.TemperatureMenuButton);

			detailsButton.Click += delegate {
				ShowDetails();
			};

			graphButton.Click += delegate {
				ShowGraph();
			};

			temperatureButton.Click += delegate {
				ShowTemperature();
			};*/
		}
		
		private void ShowGraph(){
			LinearLayout summaryButtonLayout = FindViewById<LinearLayout> (Resource.Id.GraphMenuLayout);
			LinearLayout departingView = (LinearLayout)activeButtonLayout.Parent;
			departingView.RemoveView(activeButtonLayout);

			LinearLayout summaryText = FindViewById<LinearLayout> (Resource.Id.GraphMenuText);
			ViewGroup.LayoutParams summaryParams = summaryText.LayoutParameters;

			DisplayMetrics metrics = new DisplayMetrics();         
			WindowManager.DefaultDisplay.GetMetrics(metrics);
			int height = dpiScaling.GetDPI(30);

			summaryParams.Height = height;
			summaryButtonLayout.AddView(activeButtonLayout,1);
			
			GraphFragment graphFragment = new GraphFragment ();
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, graphFragment).Commit ();
		}

		private void ShowDetails(){
			LinearLayout detailsButtonLayout = FindViewById<LinearLayout> (Resource.Id.DetailsMenuLayout);
			LinearLayout departingView = (LinearLayout)activeButtonLayout.Parent;
			departingView.RemoveView(activeButtonLayout);

			LinearLayout detailsText = FindViewById<LinearLayout> (Resource.Id.DetailsMenuText);
			ViewGroup.LayoutParams detailParams = detailsText.LayoutParameters;

			DisplayMetrics metrics = new DisplayMetrics();         
			WindowManager.DefaultDisplay.GetMetrics(metrics);
			int height = dpiScaling.GetDPI(30);

			detailParams.Height = height;
			detailsButtonLayout.AddView(activeButtonLayout,1);

			DetailsFragment detailsFragment = new DetailsFragment (dataCollection);
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, detailsFragment).Commit ();
		}

		private void ShowTemperature(){
			LinearLayout temperatureButtonLayout = FindViewById<LinearLayout> (Resource.Id.TemperatureMenuLayout);
			LinearLayout departingView = (LinearLayout)activeButtonLayout.Parent;
			departingView.RemoveView(activeButtonLayout);

			LinearLayout temperatureText = FindViewById<LinearLayout> (Resource.Id.TemeratureMenuText);
			ViewGroup.LayoutParams temperatureParams = temperatureText.LayoutParameters;

			DisplayMetrics metrics = new DisplayMetrics();         
			WindowManager.DefaultDisplay.GetMetrics(metrics);
			int height = dpiScaling.GetDPI(30);

			temperatureParams.Height = height;
			temperatureButtonLayout.AddView(activeButtonLayout,1);

			TemperatureFragment temperatureFragment = new TemperatureFragment (dataCollection);
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, temperatureFragment).Commit ();
		}
	}
}

