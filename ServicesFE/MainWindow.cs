using System;
using Gtk;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using System.Text;

public partial class MainWindow: Gtk.Window
{
	ListStore tubeStore;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
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
		// This line to trust the cert from trinity
		ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
		WebClient client = new WebClient ();
		client.Headers ["Content-type"] = "application/json";
		client.Headers ["Authorization"] = "Token token=\"8b436ea385a33c0605ebe5fcbcee4cfc\"";

		byte[] data = client.DownloadData ("https://trinity-prod-alt.abaqis.int/beanstalk/tubes");

		String s = Encoding.ASCII.GetString (data);

		StringReader treader = new StringReader (s);
		JsonReader reader = new JsonTextReader (treader);

		JsonSerializer serializer = new JsonSerializer ();
		List<String> tubes = (List<String>)serializer.Deserialize (reader, typeof(List<String>));

		tubeStore.Clear();
		foreach (String tube in tubes) {
			tubeStore.AppendValues (tube);
		}
	}
}
