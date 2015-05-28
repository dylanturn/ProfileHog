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
	public class DPIScaling
	{
		Context ParentContext;
		public DPIScaling (Context parentContext)
		{
			ParentContext = parentContext;
		}
		public  int GetDPI(int size){
			DisplayMetrics metrics = ParentContext.Resources.DisplayMetrics;
			return (size * Convert.ToInt32(metrics.DensityDpi)) / Convert.ToInt32(DisplayMetrics.DensityDefault); 
		}
	}
}

