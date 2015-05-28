package md56bb0d634f8e536edeba3342d13f3ff08;


public class UpdateProcesses
	extends android.app.ListActivity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ProfileHG.UpdateProcesses, ProfileHG, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", UpdateProcesses.class, __md_methods);
	}


	public UpdateProcesses () throws java.lang.Throwable
	{
		super ();
		if (getClass () == UpdateProcesses.class)
			mono.android.TypeManager.Activate ("ProfileHG.UpdateProcesses, ProfileHG, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public UpdateProcesses (md56bb0d634f8e536edeba3342d13f3ff08.DataCollection p0, android.view.View p1, android.app.Activity p2, android.content.Context p3) throws java.lang.Throwable
	{
		super ();
		if (getClass () == UpdateProcesses.class)
			mono.android.TypeManager.Activate ("ProfileHG.UpdateProcesses, ProfileHG, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "ProfileHG.DataCollection, ProfileHG, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
