using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
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
	public class DetailsFragment : Fragment
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

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			return inflater.Inflate(Resource.Layout.DetailsLayout, container, false);
		}

		public override void OnActivityCreated(Bundle savedInstanceState){
			base.OnActivityCreated (savedInstanceState);

			StartContentUpdate ();

			Task UIUpdateTask = new Task( () => StartUIUpdate () ,TaskCreationOptions.LongRunning);
			UIUpdateTask.Start();
		}

		public DetailsFragment (DataCollection collection)
		{
			dataCollection = collection;
		}

		public void StartContentUpdate(){
			cpuCurrentView = this.Activity.FindViewById<TextView> (Resource.Id.cpuCurrent);
			ramCurrentView = this.Activity.FindViewById<TextView> (Resource.Id.ramCurrent);
			diskReadCurrentView = this.Activity.FindViewById<TextView> (Resource.Id.diskReadCurrent);
			diskWriteCurrentView = this.Activity.FindViewById<TextView> (Resource.Id.diskWriteCurrent);
			gpuCoreCurrentView = this.Activity.FindViewById<TextView> (Resource.Id.gpuCoreCurrent);
			gpuMEMCurrentView = this.Activity.FindViewById<TextView> (Resource.Id.gpuMemoryCurrent);

			cpuHighestView = this.Activity.FindViewById<TextView> (Resource.Id.cpuHighest);
			ramHighestView = this.Activity.FindViewById<TextView> (Resource.Id.ramHighest);
			diskReadHighestView = this.Activity.FindViewById<TextView> (Resource.Id.diskReadHighest);
			diskWriteHighestView = this.Activity.FindViewById<TextView> (Resource.Id.diskWriteHighest);
			gpuCoreHighestView = this.Activity.FindViewById<TextView> (Resource.Id.gpuCoreHighest);
			gpuMEMHighestView = this.Activity.FindViewById<TextView> (Resource.Id.gpuMemoryHighest);	

			cpuAverageView = this.Activity.FindViewById<TextView> (Resource.Id.cpuAverage);
			ramAverageView = this.Activity.FindViewById<TextView> (Resource.Id.ramAverage);
			diskReadAverageView = this.Activity.FindViewById<TextView> (Resource.Id.diskReadAverage);
			diskWriteAverageView = this.Activity.FindViewById<TextView> (Resource.Id.diskWriteAverage);
			gpuCoreAverageView = this.Activity.FindViewById<TextView> (Resource.Id.gpuCoreAverage);
			gpuMEMAverageView = this.Activity.FindViewById<TextView> (Resource.Id.gpuMemoryAverage);
		}

		public void StartUIUpdate(){
			while (true) {

				this.Activity.RunOnUiThread (() => cpuCurrentView.Text = dataCollection.cpuCounter.getCurrentValue ().ToString ());
				this.Activity.RunOnUiThread (() => ramCurrentView.Text = dataCollection.ramCounter.getCurrentValue ().ToString ());
				this.Activity.RunOnUiThread (() => diskReadCurrentView.Text = dataCollection.diskReadCounter.getCurrentValue ().ToString ());
				this.Activity.RunOnUiThread (() => diskWriteCurrentView.Text = dataCollection.diskWriteCounter.getCurrentValue ().ToString ());
				this.Activity.RunOnUiThread (() => gpuCoreCurrentView.Text = dataCollection.gpuCoreCounter.getCurrentValue ().ToString ());
				this.Activity.RunOnUiThread (() => gpuMEMCurrentView.Text = dataCollection.gpuMEMCounter.getCurrentValue ().ToString ());

				this.Activity.RunOnUiThread (() => cpuHighestView.Text = dataCollection.cpuCounter.getHighestValue ().ToString ());
				this.Activity.RunOnUiThread (() => ramHighestView.Text = dataCollection.ramCounter.getHighestValue ().ToString ());
				this.Activity.RunOnUiThread (() => diskReadHighestView.Text = dataCollection.diskReadCounter.getHighestValue ().ToString ());
				this.Activity.RunOnUiThread (() => diskWriteHighestView.Text = dataCollection.diskWriteCounter.getHighestValue ().ToString ());
				this.Activity.RunOnUiThread (() => gpuCoreHighestView.Text = dataCollection.gpuCoreCounter.getHighestValue ().ToString ());
				this.Activity.RunOnUiThread (() => gpuMEMHighestView.Text = dataCollection.gpuMEMCounter.getHighestValue ().ToString ());

				this.Activity.RunOnUiThread (() => cpuAverageView.Text = dataCollection.cpuCounter.getAverageValue ().ToString ());
				this.Activity.RunOnUiThread (() => ramAverageView.Text = dataCollection.ramCounter.getAverageValue ().ToString ());
				this.Activity.RunOnUiThread (() => diskReadAverageView.Text = dataCollection.diskReadCounter.getAverageValue ().ToString ());
				this.Activity.RunOnUiThread (() => diskWriteAverageView.Text = dataCollection.diskWriteCounter.getAverageValue ().ToString ());
				this.Activity.RunOnUiThread (() => gpuCoreAverageView.Text = dataCollection.gpuCoreCounter.getAverageValue ().ToString ());
				this.Activity.RunOnUiThread (() => gpuMEMAverageView.Text = dataCollection.gpuMEMCounter.getAverageValue ().ToString ());

				Thread.Sleep (500);
			}
		}

	}
}

