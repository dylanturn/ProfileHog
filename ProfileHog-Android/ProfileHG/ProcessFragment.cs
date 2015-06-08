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

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.ProcessesLayout, container,false);
		}

		ProcessListItem FindItemByName (uint id){
			foreach (ProcessListItem pItem in ProcessList) {
				if (pItem.processName.Id == id) {
					return pItem;
				}
			}
			return (new ProcessListItem ());
		}

		public void UpdateProcessList(DataCollection collection){
			dataCollection = collection;
			LinearLayout ProcessesList = this.Activity.FindViewById<LinearLayout> (Resource.Id.ProcessListLayout);
			try{
				while(true){
					foreach (ProcessInformation process in dataCollection.processInformation) {
						
						if (process.Name.Length > 20) {
							process.Name = process.Name.Substring (0, 17) + "...";
						}

						ProcessListItem thisItem1 = FindItemByName(process.Id);

						if (thisItem1.processName != null) {

							Console.WriteLine("Updating: " + thisItem1.processName.Text);
							ProcessListItem thisItem = (ProcessListItem) thisItem1;
							this.Activity.RunOnUiThread(()=> thisItem.UpdateProcesses (process.CpuUsed.ToString(), process.RamUsed.ToString(), process.DiskUsed.ToString(), process.NetworkUsed.ToString()));
						
						} else {
							
							ProcessListItem newItem1 = new ProcessListItem (process.Name, process.Id, this.Activity);
							newItem1.UpdateProcesses (process.CpuUsed.ToString(), process.RamUsed.ToString(), process.DiskUsed.ToString(), process.NetworkUsed.ToString());

							TableRow processRow = new TableRow(this.Activity);
							TableRow.LayoutParams parms = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, 50); //Width, Height
							processRow.LayoutParameters = parms;
							processRow.SetGravity(GravityFlags.Center);

							processRow.AddView (newItem1.processName);
							processRow.AddView (newItem1.processCPU);
							processRow.AddView (newItem1.processRAM);
							processRow.AddView (newItem1.processDisk);
							processRow.AddView (newItem1.processNetwork);

							Console.WriteLine("Adding: " + newItem1.processName.Text);
							ProcessList.Add(newItem1);
							this.Activity.RunOnUiThread(()=> ProcessesList.AddView (processRow));
						}
					}
					System.Threading.Thread.Sleep (500);
				}
			}
			catch(Exception error){
				Console.WriteLine ("ERROR - " + error.Message + " " + error.StackTrace);
			}
		}
	}
}

