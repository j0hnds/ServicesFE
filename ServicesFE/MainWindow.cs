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
using System.Collections.Specialized;

public partial class MainWindow: Gtk.Window
{
	private ListStore tubeStore;
	private ListStore statisticsStore;
	private ListStore servicesStore;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();

		// Set up at least one of the server configurations
		WebServiceConfigurations.Instance.Add (new WebServiceConfiguration ("Trinity", "https://trinity-prod-alt.abaqis.int", "8b436ea385a33c0605ebe5fcbcee4cfc"));

		tubeStore = new ListStore (typeof(string));
		cbTubes.Model = tubeStore;

		TreeViewColumn statNameCol = new TreeViewColumn ();
		statNameCol.Title = "Statistic";
		CellRendererText statNameCell = new CellRendererText ();
		statNameCol.PackStart (statNameCell, true);
		statNameCol.AddAttribute (statNameCell, "text", 0);

		TreeViewColumn statValueCol = new TreeViewColumn ();
		statValueCol.Title = "Value";
		CellRendererText statValueCell = new CellRendererText();
		statValueCol.PackStart (statValueCell, true);
		statValueCol.AddAttribute (statValueCell, "text", 1);

		statisticsTree.AppendColumn (statNameCol);
		statisticsTree.AppendColumn (statValueCol);

		statisticsStore = new ListStore (typeof(string), typeof(string));
		statisticsTree.Model = statisticsStore;

		TreeViewColumn svcNameCol = new TreeViewColumn ();
		svcNameCol.Title = "Name";
		CellRendererText svcNameCell = new CellRendererText ();
		svcNameCol.PackStart (svcNameCell, true);
		svcNameCol.AddAttribute (svcNameCell, "text", 1);

		TreeViewColumn svcKeyCol = new TreeViewColumn ();
		svcKeyCol.Title = "Key";
		CellRendererText svcKeyCell = new CellRendererText ();
		svcKeyCol.PackStart (svcKeyCell, true);
		svcKeyCol.AddAttribute (svcKeyCell, "text", 2);

		servicesTree.AppendColumn (svcNameCol);
		servicesTree.AppendColumn (svcKeyCol);

		servicesStore = new ListStore (typeof(int), typeof(string), typeof(string));
		servicesTree.Model = servicesStore;

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

	protected void OnGetStatistics (object sender, EventArgs e)
	{
		Dictionary<string,string> statistics = null;

		WebServiceClient wsc = new WebServiceClient();

		// Is there a tube selected?
		string selectedTube = cbTubes.ActiveText;
		if (selectedTube != null) {
			string encodedTube = base64urlencode (Encoding.ASCII.GetBytes(selectedTube));
			statistics = wsc.DoGetDictionary ("beanstalk/tubes/" + encodedTube);
		} else {
			statistics = wsc.DoGetDictionary ("beanstalk");
		}

		statisticsStore.Clear ();
		foreach (string key in statistics.Keys) {
			statisticsStore.AppendValues (key, statistics [key]);
		}

	}

	static string base64urlencode(byte [] arg)
	{
		string s = Convert.ToBase64String(arg); // Regular base64 encoder
		// s = s.Split('=')[0]; // Remove any trailing '='s
		s = s.Replace('+', '-'); // 62nd char of encoding
		s = s.Replace('/', '_'); // 63rd char of encoding
		return s;
	}

	protected void OnNewService (object sender, EventArgs e)
	{
		ServiceDialog dlg = new ServiceDialog ();
		dlg.Modal = true;

		int response = dlg.Run ();

		if (response == (int)ResponseType.Ok) {
			// Actually save the new service
			string name = dlg.ServiceName;
			string key = dlg.ServiceKey;

			NameValueCollection nvc = new NameValueCollection ();
			nvc.Add ("service[name]", name);
			nvc.Add ("service[key]", key);

			WebServiceClient wsc = new WebServiceClient ();
			Dictionary<string,string> postResponse = wsc.DoPost ("services/services", nvc);
		} 
		dlg.Destroy ();
	}

	protected void OnEditService (object sender, EventArgs e)
	{
		TreeSelection selection = servicesTree.Selection;
		TreeIter iter;
		// selection.GetSelected(
		if (selection.GetSelected (out iter)) {
			int i = (int)servicesStore.GetValue (iter, 0);
			string name = (string)servicesStore.GetValue (iter, 1);
			string key = (string)servicesStore.GetValue (iter, 2);

			ServiceDialog dlg = new ServiceDialog ();
			dlg.Modal = true;

			dlg.ServiceKey = key;
			dlg.ServiceName = name;

			int response = dlg.Run ();
			if (response == (int)ResponseType.Ok) {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("service[name]", dlg.ServiceName);
				nvc.Add ("service[key]", dlg.ServiceKey);

				WebServiceClient wsc = new WebServiceClient ();
				Dictionary<string,string> postResponse = wsc.DoPut ("services/services/" + i, nvc);
			}
			dlg.Destroy ();
		}
	}
		
	protected void LoadServices()
	{
		servicesStore.Clear ();

		WebServiceClient wsc = new WebServiceClient ();

		List<Dictionary<string,string>> services = wsc.DoGetDictionaryList ("services/services");

		foreach (Dictionary<string,string> svc in services) {
			servicesStore.AppendValues (Convert.ToInt32(svc["id"]), svc ["name"], svc ["key"]);
		}
	}

	protected void OnSwitchPage (object o, SwitchPageArgs args)
	{
		switch (nbTabs.CurrentPage) {
		case 0: // Beanstalk
			// Nothing
			break;
		case 1: // Services
			LoadServices ();
			break;
		}
	}

	protected void OnServiceActivated (object o, RowActivatedArgs args)
	{
		TreeSelection selection = servicesTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			int i = (int)servicesStore.GetValue (iter, 0);
		}
	}


}
