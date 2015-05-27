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
		Activity ParentActivity;


		public UpdateProcesses (View ParentView, Context parentActivity)
		{
			ProcessesList = ParentView.FindViewById<LinearLayout> (Resource.Id.ProcessListLayout);
			LinearLayout newItem = new LinearLayout (parentActivity);

			LinearLayout.LayoutParams parms = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, 200); //Width, Height
			newItem.LayoutParameters = parms;
			TextView asdf = new TextView (parentActivity);
			asdf.Text = "Hello!";
			newItem.AddView (asdf);
			ParentActivity.RunOnUiThread (() => ProcessesList.AddView(newItem));
		}
	}
}

