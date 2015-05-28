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
	public class UpdateProcesses : ListActivity
	{

		LinearLayout ProcessesList;
		TableRow HeaderRow;

		Activity ParentActivity;
		View ParentView;
		Context ParentContext;
		DataCollection ParentCollection;


		public UpdateProcesses (DataCollection collection, View parentView, Activity parentActivity, Context parentContext)
		{
			ParentCollection = collection;
			ParentActivity = parentActivity;
			ParentView = parentView;
			ParentContext = parentContext;

			ProcessesList = ParentView.FindViewById<LinearLayout> (Resource.Id.ProcessListLayout);
			HeaderRow = ProcessesList.FindViewById<TableRow> (Resource.Id.HeaderRow);
		}

		public void StartUIUpdate(){
			while (true) {
				try{
				Console.WriteLine ("Listing processes");

				ParentActivity.RunOnUiThread (() => ProcessesList.RemoveAllViews ());
				ParentActivity.RunOnUiThread (() => ProcessesList.AddView (HeaderRow));
				
				Console.WriteLine ("Header Row Cleared");
				
				foreach (ProcessInformation thisProcess in ParentCollection.processInformation.ToArray()) {
						Console.WriteLine ("Adding new Row");
						TableRow processRow = new TableRow(ParentContext);
					TableRow.LayoutParams parms = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, 50); //Width, Height

					processRow.LayoutParameters = parms;

					processRow.SetGravity(GravityFlags.Center);
					TextView processName = new TextView (ParentContext);
					TableRow.LayoutParams textParams = new TableRow.LayoutParams ();
					
					
						DPIScaling dpiScaling = new DPIScaling(ParentContext);
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

					TextView processCPU = new TextView (ParentContext);
					processCPU.SetTextSize (ComplexUnitType.Dip, 12);
					processCPU.Text = thisProcess.CpuUsed.ToString();
					processCPU.Gravity = GravityFlags.Center;

					TextView processRAM = new TextView (ParentContext);
					processRAM.SetTextSize (ComplexUnitType.Dip, 12);
					processRAM.Text = thisProcess.RamUsed.ToString();
					processRAM.Gravity = GravityFlags.Center;
										
					TextView processDisk = new TextView (ParentContext);
					processDisk.SetTextSize (ComplexUnitType.Dip, 12);
					processDisk.Text = thisProcess.DiskUsed.ToString();
					processDisk.Gravity = GravityFlags.Center;

					TextView processNetwork = new TextView (ParentContext);
					processNetwork.SetTextSize (ComplexUnitType.Dip, 12);
					processNetwork.Text = thisProcess.NetworkUsed.ToString();
					processNetwork.Gravity = GravityFlags.Center;
					
						Console.WriteLine ("Adding textview to the row");
					processRow.AddView (processName);
					processRow.AddView (processCPU);
					processRow.AddView (processRAM);
					processRow.AddView (processDisk);
					processRow.AddView (processNetwork);
						Console.WriteLine ("Adding the row to the list view");
					ParentActivity.RunOnUiThread (() => ProcessesList.AddView (processRow));

				}
				Thread.Sleep (500);
				}catch(Exception error){
					Console.WriteLine ("PROCESS LIST ERROR: " + error.Message);
				}
			}
		}
	}
}

