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
		DataCollection dataCollection = new DataCollection ();
		Task uiUpdateTask;
		LinearLayout activeButtonLayout;
		DPIScaling dpiScaling;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			SetContentView (Resource.Layout.Main);

			dataCollection.OnStartCommand (this.Intent, StartCommandFlags.Retry, 0);
			activeButtonLayout = FindViewById<LinearLayout> (Resource.Id.activeButtonLayout);

			dpiScaling = new DPIScaling (this);

			ShowGraph ();



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

			FrameLayout frameLayout = FindViewById<FrameLayout> (Resource.Id.frameLayout1);
			frameLayout.RemoveAllViews ();
			LayoutInflater.Inflate(Resource.Layout.GraphLayout, frameLayout,true);
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

			FrameLayout frameLayout = FindViewById<FrameLayout> (Resource.Id.frameLayout1);
			frameLayout.RemoveAllViews ();
			LayoutInflater.Inflate(Resource.Layout.DetailsLayout, frameLayout,true);

			LinearLayout rootLayout = FindViewById<LinearLayout> (Resource.Id.linearLayout1);
			UpdateDetails updateDetails = new UpdateDetails (dataCollection, rootLayout, this);

			uiUpdateTask = new Task( () => updateDetails.StartUIUpdate(),TaskCreationOptions.LongRunning);
			uiUpdateTask.Start();
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

			FrameLayout frameLayout = FindViewById<FrameLayout> (Resource.Id.frameLayout1);
			frameLayout.RemoveAllViews ();
			LayoutInflater.Inflate(Resource.Layout.ProcessesLayout, frameLayout,true);

			UpdateProcesses updateProcesses = new UpdateProcesses (dataCollection, frameLayout, this, this);
			uiUpdateTask = new Task( () => updateProcesses.StartUIUpdate(),TaskCreationOptions.LongRunning);
			uiUpdateTask.Start();
			//CreateListView (dataCollection.processInformation);
		}

		private void CreateListView(List<ProcessInformation> newProcesses){
			Console.WriteLine ("Listing processes");
			LinearLayout ProcessesList = FindViewById<LinearLayout> (Resource.Id.ProcessListLayout);
			TableRow HeaderRow = ProcessesList.FindViewById<TableRow> (Resource.Id.HeaderRow);
			ProcessesList.RemoveAllViews ();
			ProcessesList.AddView (HeaderRow);
			foreach (ProcessInformation thisProcess in newProcesses.ToArray()) {
				TableRow processRow = new TableRow(this);
				TableRow.LayoutParams parms = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, 50); //Width, Height

				processRow.LayoutParameters = parms;

				processRow.SetGravity(GravityFlags.Center);
				TextView processName = new TextView (this);
				TableRow.LayoutParams textParams = new TableRow.LayoutParams ();
				DisplayMetrics metrics = new DisplayMetrics();         
				WindowManager.DefaultDisplay.GetMetrics(metrics);
				textParams.LeftMargin = dpiScaling.GetDPI (5);

				if (thisProcess.Name.Length > 20) {
					processName.Text = thisProcess.Name.Substring (0, 17) + "...";
					processName.LayoutParameters = textParams;
					processName.SetTextSize (ComplexUnitType.Dip, 12);
				} else {
					processName.Text = thisProcess.Name;
					processName.LayoutParameters = textParams;
					processName.SetTextSize (ComplexUnitType.Dip, 12);
				}

				TextView processCPU = new TextView (this);
				processCPU.SetTextSize (ComplexUnitType.Dip, 12);
				processCPU.Text = thisProcess.CpuUsed.ToString();
				processCPU.Gravity = GravityFlags.Center;

				TextView processRAM = new TextView (this);
				processRAM.SetTextSize (ComplexUnitType.Dip, 12);
				processRAM.Text = thisProcess.RamUsed.ToString();
				processRAM.Gravity = GravityFlags.Center;

				TextView processDisk = new TextView (this);
				processDisk.SetTextSize (ComplexUnitType.Dip, 12);
				processDisk.Text = thisProcess.DiskUsed.ToString();
				processDisk.Gravity = GravityFlags.Center;

				TextView processNetwork = new TextView (this);
				processNetwork.SetTextSize (ComplexUnitType.Dip, 12);
				processNetwork.Text = thisProcess.NetworkUsed.ToString();
				processNetwork.Gravity = GravityFlags.Center;

				processRow.AddView (processName);
				processRow.AddView (processCPU);
				processRow.AddView (processRAM);
				processRow.AddView (processDisk);
				processRow.AddView (processNetwork);

				ProcessesList.AddView (processRow);
			}
		}
	}
}

