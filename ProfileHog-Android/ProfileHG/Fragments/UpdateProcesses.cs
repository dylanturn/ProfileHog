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
		List<ProcessListItem> AllProcesses;

		Activity ParentActivity;
		View ParentView;
		Context ParentContext;
		DataCollection ParentCollection;

		struct ProcessListItem{
			public static TextView processName;
			public TextView processCPU;
			public TextView processRAM;
			public TextView processDisk;
			public TextView processNetwork;

			public ProcessListItem(string ProcessName){

				TextView processName = new TextView (ParentContext);
				TableRow.LayoutParams textParams = new TableRow.LayoutParams ();

				if (ProcessName.Length > 20) {
					processName.Text = ProcessName.Substring (0, 17) + "...";
					processName.LayoutParameters = textParams;
					processName.SetTextSize (ComplexUnitType.Dip, 12);
				} else {
					processName.Text = ProcessName;
					processName.LayoutParameters = textParams;
					processName.SetTextSize (ComplexUnitType.Dip, 12);
				}

				TextView processCPU = new TextView (ParentContext);
				processCPU.SetTextSize (ComplexUnitType.Dip, 12);
				processCPU.Gravity = GravityFlags.Center;

				TextView processRAM = new TextView (ParentContext);
				processRAM.SetTextSize (ComplexUnitType.Dip, 12);
				processRAM.Gravity = GravityFlags.Center;

				TextView processDisk = new TextView (ParentContext);
				processDisk.SetTextSize (ComplexUnitType.Dip, 12);
				processDisk.Gravity = GravityFlags.Center;

				TextView processNetwork = new TextView (ParentContext);
				processNetwork.SetTextSize (ComplexUnitType.Dip, 12);
				processNetwork.Gravity = GravityFlags.Center;

			}

			public void UpdateProcesses (Activity ParentActivity, string CPUValue, string RAMValue, string DISKValue, string NETValue){
				ParentActivity.RunOnUiThread (() => processCPU.Text = CPUValue);
				ParentActivity.RunOnUiThread (() => processRAM.Text = RAMValue);
				ParentActivity.RunOnUiThread (() => processDisk.Text = DISKValue);
				ParentActivity.RunOnUiThread (() => processNetwork.Text = NETValue);
			}
			public void AddProcesses (Activity ParentActivity, string CPUValue, string RAMValue, string DISKValue, string NETValue){
				ParentActivity.RunOnUiThread (() => processCPU.Text = CPUValue);
				ParentActivity.RunOnUiThread (() => processRAM.Text = RAMValue);
				ParentActivity.RunOnUiThread (() => processDisk.Text = DISKValue);
				ParentActivity.RunOnUiThread (() => processNetwork.Text = NETValue);
			}
			public void RemoveProcess(string ProcessName){
				
			}
		}

		public UpdateProcesses (DataCollection collection, View parentView, Activity parentActivity, Context parentContext)
		{
			ParentCollection = collection;
			ParentActivity = parentActivity;
			ParentView = parentView;
			ParentContext = parentContext;

			ProcessesList = ParentView.FindViewById<LinearLayout> (Resource.Id.ProcessListLayout);
			HeaderRow = ProcessesList.FindViewById<TableRow> (Resource.Id.HeaderRow);
		}

		public void UpdateProcessInfo(){
			foreach (ProcessInformation thisProcess in ParentCollection.processInformation.ToArray()) {
				ProcessListItem value = AllProcesses.Find (newItem => newItem.processName.Text == thisProcess.Name);
				if (value.processName.Text != "") {
					value.UpdateProcesses (ParentActivity, thisProcess.CpuUsed.ToString(), thisProcess.RamUsed.ToString(), thisProcess.DiskUsed.ToString(), thisProcess.NetworkUsed.ToString());
				} else {
					ProcessListItem newProcessItem = new ProcessListItem (thisProcess.Name);
					newProcessItem.UpdateProcesses (ParentActivity, thisProcess.CpuUsed.ToString(), thisProcess.RamUsed.ToString(), thisProcess.DiskUsed.ToString(), thisProcess.NetworkUsed.ToString());
				}
			}
		}

		public void StartUIUpdate(){
			while (true) {
				try{
					foreach (ProcessInformation thisProcess in ParentCollection.processInformation.ToArray()) {
						ProcessListItem value = AllProcesses.Find(newItem => newItem.processName.Text == thisProcess.Name);
						if(value.processName.Text != ""){
							ParentActivity.RunOnUiThread (() => value.UpdateProcesses(ParentActivity, thisProcess.CpuUsed.ToString(),thisProcess.RamUsed.ToString(), thisProcess.DiskUsed.ToString(), thisProcess.NetworkUsed.ToString()));
						}
				}
				}catch(Exception error){
					Console.WriteLine ("PROCESS LIST ERROR: " + error.Message);
				}
				Thread.Sleep (500);
			}
		}

		public void CreateTable(){
			try{
				
				foreach (ProcessListItem thisProcess in AllProcesses) {
					
					Console.WriteLine ("Adding new Row");
					TableRow processRow = new TableRow(ParentContext);
					TableRow.LayoutParams parms = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, 50); //Width, Height
					processRow.LayoutParameters = parms;
					processRow.SetGravity(GravityFlags.Center);

					Console.WriteLine ("Adding textview to the row");
					processRow.AddView (thisProcess.processName);
					processRow.AddView (thisProcess.processCPU);
					processRow.AddView (thisProcess.processRAM);
					processRow.AddView (thisProcess.processDisk);
					processRow.AddView (thisProcess.processNetwork);

					Console.WriteLine ("Adding the row to the list view");
					ParentActivity.RunOnUiThread (() => ProcessesList.AddView (processRow));

				}

			}catch(Exception error){
				
				Console.WriteLine ("PROCESS LIST ERROR: " + error.Message);

			}
		}
	}
}

