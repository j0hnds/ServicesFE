using System;
using Gtk;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using System.Text;
using ServicesFE;

public partial class MainWindow: Gtk.Window
{
	ListStore tubeStore;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();

		// Set up at least one of the server configurations
		WebServiceConfigurations.Instance.Add (new WebServiceConfiguration ("Trinity", "https://trinity-prod-alt.abaqis.int", "8b436ea385a33c0605ebe5fcbcee4cfc"));

		tubeStore = new ListStore (typeof(string));
		cbTubes.Model = tubeStore;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExit (object sender, EventArgs e)
	{
		Application.Quit ();
	}

	protected void OnRefreshTubes (object sender, EventArgs e)
	{
		WebServiceClient wsc = new WebServiceClient();

		List<string> tubes = wsc.DoGetList ("beanstalk/tubes");

		tubeStore.Clear();
		foreach (String tube in tubes) {
			tubeStore.AppendValues (tube);
		}
	}
}
