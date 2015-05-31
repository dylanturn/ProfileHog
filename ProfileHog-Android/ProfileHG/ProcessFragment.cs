
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
	public class ProcessFragment : Fragment
	{
		DataCollection dataCollection;
		List<ProcessListItem> ProcessList = new List<ProcessListItem>();

		struct ProcessListItem{
			public TextView processName;
			public TextView processCPU;
			public TextView processRAM;
			public TextView processDisk;
			public TextView processNetwork;

			public ProcessListItem(string ProcessName, Context thisContext){

				processName = new TextView (thisContext);
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

				processCPU = new TextView (thisContext);
				processCPU.SetTextSize (ComplexUnitType.Dip, 12);
				processCPU.Gravity = GravityFlags.Center;

				processRAM = new TextView (thisContext);
				processRAM.SetTextSize (ComplexUnitType.Dip, 12);
				processRAM.Gravity = GravityFlags.Center;

				processDisk = new TextView (thisContext);
				processDisk.SetTextSize (ComplexUnitType.Dip, 12);
				processDisk.Gravity = GravityFlags.Center;

				processNetwork = new TextView (thisContext);
				processNetwork.SetTextSize (ComplexUnitType.Dip, 12);
				processNetwork.Gravity = GravityFlags.Center;

			}

			public void UpdateProcesses (string CPUValue, string RAMValue, string DISKValue, string NETValue){
				processCPU.Text = CPUValue;
				processRAM.Text = RAMValue;
				processDisk.Text = DISKValue;
				processNetwork.Text = NETValue;
			}

			public void RemoveProcess(string ProcessName){

			}
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.ProcessesLayout, container,false);
		}

		public override void OnActivityCreated(Bundle savedInstanceState){
			base.OnActivityCreated(savedInstanceState);
			// Create your fragment here

		}

		public void UpdateProcessList(DataCollection collection){
			dataCollection = collection;
			try{
				while(true){
			LinearLayout ProcessesList = this.Activity.FindViewById<LinearLayout> (Resource.Id.ProcessListLayout);

			foreach (ProcessInformation process in dataCollection.processInformation) {
				ProcessListItem thisItem1 = ProcessList.Find (newItem => newItem.processName.Text == process.Name);

						if (thisItem1.processName != null) {
					ProcessListItem thisItem = (ProcessListItem) thisItem1;
					thisItem.UpdateProcesses (process.CpuUsed.ToString(), process.RamUsed.ToString(), process.DiskUsed.ToString(), process.NetworkUsed.ToString());
				} else {
					ProcessListItem newItem1 = new ProcessListItem (process.Name, this.Activity);
					newItem1.UpdateProcesses (process.CpuUsed.ToString(), process.RamUsed.ToString(), process.DiskUsed.ToString(), process.NetworkUsed.ToString());

					Console.WriteLine ("Adding new Row");
					TableRow processRow = new TableRow(this.Activity);
					TableRow.LayoutParams parms = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, 50); //Width, Height
					processRow.LayoutParameters = parms;
					processRow.SetGravity(GravityFlags.Center);

					Console.WriteLine ("Adding textview to the row");
					processRow.AddView (newItem1.processName);
					processRow.AddView (newItem1.processCPU);
					processRow.AddView (newItem1.processRAM);
					processRow.AddView (newItem1.processDisk);
					processRow.AddView (newItem1.processNetwork);

					Console.WriteLine ("Adding the row to the list view");
					ProcessList.Add(newItem1);
					this.Activity.RunOnUiThread(()=> ProcessesList.AddView (processRow));
				}
			}
			System.Threading.Thread.Sleep (1000);
				}
			}
			catch(Exception error){
				Console.WriteLine ("ERROR - " + error.Message);
			}
		}
	}
}

