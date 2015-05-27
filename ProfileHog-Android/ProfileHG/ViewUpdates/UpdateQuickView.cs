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

				ParentActivity.RunOnUiThread (() => cpuQuickView.Text = dataCollection.cpuCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => ramQuickView.Text = dataCollection.ramCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskWriteQuickView.Text = dataCollection.diskActiveCounter.getCurrentValue().ToString());
				ParentActivity.RunOnUiThread (() => gpuCoreQuickView.Text = dataCollection.gpuCoreCounter.getCurrentValue ().ToString ());

				Thread.Sleep (500);
			}
		}
	}
}

