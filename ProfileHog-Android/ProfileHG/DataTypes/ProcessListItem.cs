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
	public class ProcessListItem
	{
		public uint processID;
		public TextView processName;
		public TextView processCPU;
		public TextView processRAM;
		public TextView processDisk;
		public TextView processNetwork;

		public ProcessListItem(){
		}

		public ProcessListItem(string ProcessName, uint ProcessID, Context thisContext){
			processID = ProcessID;
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
}

