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
		public DataCollection dataCollection;
		public List<SensorCard> quickViewCards;

		protected override void OnCreate (Bundle bundle)
		{
			this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
			base.OnCreate (bundle);
			dataCollection = new DataCollection ();
			quickViewCards = new List<SensorCard>();
			SetContentView (Resource.Layout.ProfileHogMain);

			dataCollection.OnStartCommand (this.Intent, StartCommandFlags.Retry, 0);

			UpdateQuickView updateQuickView = new UpdateQuickView (dataCollection, this, this);
			Task QuickViewTask = new Task(() => updateQuickView.StartUIUpdate (this.LayoutInflater),TaskCreationOptions.LongRunning);
			QuickViewTask.Start ();
			
			

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
			};
		}
		
		private void ShowGraph(){
			GraphFragment graphFragment = new GraphFragment ();
			FragmentManager.BeginTransaction ().Replace (Resource.Id.ContentLayout, graphFragment).Commit ();
		}

		private void ShowDetails(){
			DetailsFragment detailsFragment = new DetailsFragment (dataCollection);
			FragmentManager.BeginTransaction ().Replace (Resource.Id.ContentLayout, detailsFragment).Commit ();
		}

		private void ShowTemperature(){
			TemperatureFragment temperatureFragment = new TemperatureFragment (dataCollection);
			FragmentManager.BeginTransaction ().Replace (Resource.Id.ContentLayout, temperatureFragment).Commit ();
		}
	}
}

