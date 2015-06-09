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
	public class UpdateDetails
	{
		TextView cpuCurrentView;
		TextView ramCurrentView;
		TextView diskReadCurrentView;
		TextView diskWriteCurrentView;
		TextView gpuCoreCurrentView;
		TextView gpuMEMCurrentView;

		TextView cpuHighestView;
		TextView ramHighestView;
		TextView diskReadHighestView;
		TextView diskWriteHighestView;
		TextView gpuCoreHighestView;
		TextView gpuMEMHighestView;

		TextView cpuAverageView;
		TextView ramAverageView;
		TextView diskReadAverageView;
		TextView diskWriteAverageView;
		TextView gpuCoreAverageView;
		TextView gpuMEMAverageView;

		DataCollection dataCollection;
		Activity ParentActivity;

		public UpdateDetails (DataCollection collection, View ParentView, Activity parentActivity)
		{
			dataCollection = collection;
			ParentActivity = parentActivity;

			cpuCurrentView = ParentView.FindViewById<TextView> (Resource.Id.cpuCurrent);
			ramCurrentView = ParentView.FindViewById<TextView> (Resource.Id.ramCurrent);
			diskReadCurrentView = ParentView.FindViewById<TextView> (Resource.Id.diskReadCurrent);
			diskWriteCurrentView = ParentView.FindViewById<TextView> (Resource.Id.diskWriteCurrent);
			gpuCoreCurrentView = ParentView.FindViewById<TextView> (Resource.Id.gpuCoreCurrent);
			gpuMEMCurrentView = ParentView.FindViewById<TextView> (Resource.Id.gpuMemoryCurrent);

			cpuHighestView = ParentView.FindViewById<TextView> (Resource.Id.cpuHighest);
			ramHighestView = ParentView.FindViewById<TextView> (Resource.Id.ramHighest);
			diskReadHighestView = ParentView.FindViewById<TextView> (Resource.Id.diskReadHighest);
			diskWriteHighestView = ParentView.FindViewById<TextView> (Resource.Id.diskWriteHighest);
			gpuCoreHighestView = ParentView.FindViewById<TextView> (Resource.Id.gpuCoreHighest);
			gpuMEMHighestView = ParentView.FindViewById<TextView> (Resource.Id.gpuMemoryHighest);	

			cpuAverageView = ParentView.FindViewById<TextView> (Resource.Id.cpuAverage);
			ramAverageView = ParentView.FindViewById<TextView> (Resource.Id.ramAverage);
			diskReadAverageView = ParentView.FindViewById<TextView> (Resource.Id.diskReadAverage);
			diskWriteAverageView = ParentView.FindViewById<TextView> (Resource.Id.diskWriteAverage);
			gpuCoreAverageView = ParentView.FindViewById<TextView> (Resource.Id.gpuCoreAverage);
			gpuMEMAverageView = ParentView.FindViewById<TextView> (Resource.Id.gpuMemoryAverage);
		}

		public void StartUIUpdate(){
			while (true) {
				
				ParentActivity.RunOnUiThread (() => cpuCurrentView.Text = dataCollection.cpuCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => ramCurrentView.Text = dataCollection.ramCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskReadCurrentView.Text = dataCollection.diskReadCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskWriteCurrentView.Text = dataCollection.diskWriteCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => gpuCoreCurrentView.Text = dataCollection.gpuCoreCounter.getCurrentValue ().ToString ());
				ParentActivity.RunOnUiThread (() => gpuMEMCurrentView.Text = dataCollection.gpuMEMCounter.getCurrentValue ().ToString ());

				ParentActivity.RunOnUiThread (() => cpuHighestView.Text = dataCollection.cpuCounter.getHighestValue ().ToString ());
				ParentActivity.RunOnUiThread (() => ramHighestView.Text = dataCollection.ramCounter.getHighestValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskReadHighestView.Text = dataCollection.diskReadCounter.getHighestValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskWriteHighestView.Text = dataCollection.diskWriteCounter.getHighestValue ().ToString ());
				ParentActivity.RunOnUiThread (() => gpuCoreHighestView.Text = dataCollection.gpuCoreCounter.getHighestValue ().ToString ());
				ParentActivity.RunOnUiThread (() => gpuMEMHighestView.Text = dataCollection.gpuMEMCounter.getHighestValue ().ToString ());

				ParentActivity.RunOnUiThread (() => cpuAverageView.Text = dataCollection.cpuCounter.getAverageValue ().ToString ());
				ParentActivity.RunOnUiThread (() => ramAverageView.Text = dataCollection.ramCounter.getAverageValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskReadAverageView.Text = dataCollection.diskReadCounter.getAverageValue ().ToString ());
				ParentActivity.RunOnUiThread (() => diskWriteAverageView.Text = dataCollection.diskWriteCounter.getAverageValue ().ToString ());
				ParentActivity.RunOnUiThread (() => gpuCoreAverageView.Text = dataCollection.gpuCoreCounter.getAverageValue ().ToString ());
				ParentActivity.RunOnUiThread (() => gpuMEMAverageView.Text = dataCollection.gpuMEMCounter.getAverageValue ().ToString ());

				Thread.Sleep (500);
			}
		}

	}
}

