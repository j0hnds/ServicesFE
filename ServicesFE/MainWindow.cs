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
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
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

		byte[] data = client.DownloadData ("https://trinity-prod-alt.abaqis.int/beanstalk");

		// MemoryStream ms = new MemoryStream (data);
		// string anS = ms.ToString ();

		String s = Encoding.ASCII.GetString (data);

		StringReader treader = new StringReader (s);
		JsonReader reader = new JsonTextReader (treader);

		JsonSerializer serializer = new JsonSerializer ();
		Dictionary<String,String> o = (Dictionary<String,String>)serializer.Deserialize (reader, typeof(Dictionary<String,String>));

		string val = o ["cmd-reserve-with-timeout"];
	}
}
