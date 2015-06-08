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

		LinearLayout activeButtonLayout;
		DPIScaling dpiScaling;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			SetContentView (Resource.Layout.Main);

			dataCollection.OnStartCommand (this.Intent, StartCommandFlags.Retry, 0);
			activeButtonLayout = FindViewById<LinearLayout> (Resource.Id.activeButtonLayout);
			dpiScaling = new DPIScaling (this);

			GraphFragment graphFragment = new GraphFragment ();
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, graphFragment).Commit ();

			Task quickViewUpdateTask = new Task( () => new UpdateQuickView (dataCollection, this).StartUIUpdate (),TaskCreationOptions.LongRunning);
			quickViewUpdateTask.Start();

			Button detailsButton = FindViewById<Button> (Resource.Id.detailsView);
			Button graphButton = FindViewById<Button> (Resource.Id.summaryView);
			Button processesButton = FindViewById<Button> (Resource.Id.ProcessesView);

			detailsButton.Click += delegate {
				ShowDetails();
			};

			graphButton.Click += delegate {
				ShowGraph();
			};

			processesButton.Click += delegate {
				ShowProcesses();
			};

		}
		
		private void ShowGraph(){
			LinearLayout summaryButtonLayout = FindViewById<LinearLayout> (Resource.Id.summaryLayout);
			LinearLayout departingView = (LinearLayout)activeButtonLayout.Parent;
			departingView.RemoveView(activeButtonLayout);

			LinearLayout summaryText = FindViewById<LinearLayout> (Resource.Id.summaryText);
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
			LinearLayout detailsButtonLayout = FindViewById<LinearLayout> (Resource.Id.detailsLayout);
			LinearLayout departingView = (LinearLayout)activeButtonLayout.Parent;
			departingView.RemoveView(activeButtonLayout);

			LinearLayout detailsText = FindViewById<LinearLayout> (Resource.Id.detailsText);
			ViewGroup.LayoutParams detailParams = detailsText.LayoutParameters;

			DisplayMetrics metrics = new DisplayMetrics();         
			WindowManager.DefaultDisplay.GetMetrics(metrics);
			int height = dpiScaling.GetDPI(30);

			detailParams.Height = height;
			detailsButtonLayout.AddView(activeButtonLayout,1);

			DetailsFragment detailsFragment = new DetailsFragment (dataCollection);
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, detailsFragment).Commit ();
		}

		private void ShowProcesses(){
			LinearLayout processesButtonLayout = FindViewById<LinearLayout> (Resource.Id.ProcessesLayout);
			LinearLayout departingView = (LinearLayout)activeButtonLayout.Parent;
			departingView.RemoveView(activeButtonLayout);

			LinearLayout processesText = FindViewById<LinearLayout> (Resource.Id.ProcessesText);
			ViewGroup.LayoutParams processesParams = processesText.LayoutParameters;

			DisplayMetrics metrics = new DisplayMetrics();         
			WindowManager.DefaultDisplay.GetMetrics(metrics);
			int height = dpiScaling.GetDPI(30);

			processesParams.Height = height;
			processesButtonLayout.AddView(activeButtonLayout,1);

			ProcessFragment processFragment = new ProcessFragment ();
			TemperatureFragment temperatureFragment = new TemperatureFragment (dataCollection);
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frameLayout1, temperatureFragment).Commit ();
		}
	}
}

