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

		TextView cpuQuickView;
		TextView ramQuickView;
		TextView diskWriteQuickView;
		TextView gpuCoreQuickView;

		DataCollection dataCollection;
		Activity ParentActivity;

		public UpdateQuickView (DataCollection collection, Activity parentActivity)
		{
			dataCollection = collection;
			ParentActivity = parentActivity;

			cpuQuickView = parentActivity.FindViewById<TextView> (Resource.Id.cpuQuickViewCurrent);
			ramQuickView = parentActivity.FindViewById<TextView> (Resource.Id.ramQuickViewCurrent);
			diskWriteQuickView = parentActivity.FindViewById<TextView> (Resource.Id.diskQuickViewCurrent);
			gpuCoreQuickView = parentActivity.FindViewById<TextView> (Resource.Id.gpuQuickViewCurrent);

		}

		public void StartUIUpdate(){
			while (true) {

				foreach (Hardware thisHardware in dataCollection.HardwareList) {
					if (thisHardware.HardwareType == Hardware.Types.CPU) {
						foreach (Sensor thisSensor in thisHardware.SensorList) {
							if (thisSensor.SensorName == "CPU Total") {
								ParentActivity.RunOnUiThread (() => cpuQuickView.Text = thisSensor.CurrentValue.getValue ().ToString("0"));
							}
						}
					}

					if (thisHardware.HardwareType == Hardware.Types.RAM) {
						foreach (Sensor thisSensor in thisHardware.SensorList) {
							if (thisSensor.SensorName == "Used Memory") {
								ParentActivity.RunOnUiThread (() => ramQuickView.Text = thisSensor.CurrentValue.getValue ().ToString("0"));
							}
						}
					}

					if (thisHardware.HardwareType == Hardware.Types.HDD) {
						foreach (Sensor thisSensor in thisHardware.SensorList) {
							if (thisSensor.SensorName == "DiskActive") {
								ParentActivity.RunOnUiThread (() => diskWriteQuickView.Text = thisSensor.CurrentValue.getValue ().ToString("0"));
							}
						}
					}

					if (thisHardware.HardwareType == Hardware.Types.GpuNvidia || thisHardware.HardwareType == Hardware.Types.GpuAti) {
						foreach (Sensor thisSensor in thisHardware.SensorList) {
							if (thisSensor.SensorName == "GPU Core") {
								ParentActivity.RunOnUiThread (() => gpuCoreQuickView.Text = thisSensor.CurrentValue.getValue ().ToString("0"));
							}
						}
					}
				}

				Thread.Sleep (500);
			}
		}
	}
}

