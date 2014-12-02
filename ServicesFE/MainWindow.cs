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
	private ListStore thirdPartiesStore;
	private ListStore publicKeysStore;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();

		// Set up at least one of the server configurations
		// WebServiceConfigurations.Instance.Add (new WebServiceConfiguration ("Local", "http://localhost:8080", "8b436ea385a33c0605ebe5fcbcee4cfc"));
		WebServiceConfigurations.Instance.Add (new WebServiceConfiguration ("Trinity", "https://trinity-prod-alt.abaqis.int", "8b436ea385a33c0605ebe5fcbcee4cfc"));

		tubeStore = new ListStore (typeof(string));
		cbTubes.Model = tubeStore;

		SetupBeanstalkTreeView ();

		SetupServicesTreeView ();

		SetupThirdPartyTreeView ();

		SetupPublicKeysTreeView ();

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

	protected void LoadThirdParties()
	{
		thirdPartiesStore.Clear ();

		WebServiceClient wsc = new WebServiceClient ();

		List<Dictionary<string,string>> thirdParties = wsc.DoGetDictionaryList ("services/third_parties");

		foreach (Dictionary<string,string> tp in thirdParties) {
			thirdPartiesStore.AppendValues (Convert.ToInt32(tp["id"]), tp ["name"], tp ["key"], tp ["contact_email"]);
		}
	}

	protected void LoadPublicKeys()
	{
		publicKeysStore.Clear ();

		WebServiceClient wsc = new WebServiceClient ();

		List<Dictionary<string,string>> publicKeys = wsc.DoGetDictionaryList ("services/public_keys");

		foreach (Dictionary<string,string> pk in publicKeys) {
			publicKeysStore.AppendValues (Convert.ToInt32(pk["id"]), pk ["name"], pk ["valid_until"]);
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
		case 2: // Third Parties
			LoadThirdParties ();
			break;

		case 3: // Public keys
			LoadPublicKeys ();
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

	private void SetupBeanstalkTreeView()
	{
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
	}

	private void SetupServicesTreeView()
	{
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

	private void SetupThirdPartyTreeView()
	{
		TreeViewColumn tpNameCol = new TreeViewColumn ();
		tpNameCol.Title = "Name";
		CellRendererText tpNameCell = new CellRendererText ();
		tpNameCol.PackStart (tpNameCell, true);
		tpNameCol.AddAttribute (tpNameCell, "text", 1);

		TreeViewColumn tpKeyCol = new TreeViewColumn ();
		tpKeyCol.Title = "Key";
		CellRendererText tpKeyCell = new CellRendererText ();
		tpKeyCol.PackStart (tpKeyCell, true);
		tpKeyCol.AddAttribute (tpKeyCell, "text", 2);

		TreeViewColumn tpEmailCol = new TreeViewColumn ();
		tpEmailCol.Title = "Email";
		CellRendererText tpEmailCell = new CellRendererText ();
		tpEmailCol.PackStart (tpEmailCell, true);
		tpEmailCol.AddAttribute (tpEmailCell, "text", 3);

		thirdPartiesTree.AppendColumn (tpNameCol);
		thirdPartiesTree.AppendColumn (tpKeyCol);
		thirdPartiesTree.AppendColumn (tpEmailCol);

		thirdPartiesStore = new ListStore (typeof(int), typeof(string), typeof(string), typeof(string));
		thirdPartiesTree.Model = thirdPartiesStore;
	}

	private void SetupPublicKeysTreeView()
	{
		TreeViewColumn pkNameCol = new TreeViewColumn ();
		pkNameCol.Title = "Name";
		CellRendererText pkNameCell = new CellRendererText ();
		pkNameCol.PackStart (pkNameCell, true);
		pkNameCol.AddAttribute (pkNameCell, "text", 1);

		TreeViewColumn pkValidUntilCol = new TreeViewColumn ();
		pkValidUntilCol.Title = "Valid Until";
		CellRendererText pkValidUntilCell = new CellRendererText ();
		pkValidUntilCol.PackStart (pkValidUntilCell, true);
		pkValidUntilCol.AddAttribute (pkValidUntilCell, "text", 2);

		publicKeysTree.AppendColumn (pkNameCol);
		publicKeysTree.AppendColumn (pkValidUntilCol);

		publicKeysStore = new ListStore (typeof(int), typeof(string), typeof(string));
		publicKeysTree.Model = publicKeysStore;
	}

	protected void OnNewThirdParty (object sender, EventArgs e)
	{
		ThirdPartyDialog dlg = new ThirdPartyDialog ();
		dlg.Modal = true;

		int response = dlg.Run ();

		if (response == (int)ResponseType.Ok) {
			// Actually save the new service
			string name = dlg.ThirdPartyName;
			string key = dlg.ThirdPartyKey;
			string email = dlg.ThirdPartyEmail;

			NameValueCollection nvc = new NameValueCollection ();
			nvc.Add ("third_party[name]", name);
			nvc.Add ("third_party[key]", key);
			nvc.Add ("third_party[contact_email]", email);

			WebServiceClient wsc = new WebServiceClient ();
			Dictionary<string,string> postResponse = wsc.DoPost ("services/third_parties", nvc);
		} 
		dlg.Destroy ();
	}

	protected void OnEditThirdParty (object sender, EventArgs e)
	{
		TreeSelection selection = thirdPartiesTree.Selection;
		TreeIter iter;
		// selection.GetSelected(
		if (selection.GetSelected (out iter)) {
			int i = (int)thirdPartiesStore.GetValue (iter, 0);
			string name = (string)thirdPartiesStore.GetValue (iter, 1);
			string key = (string)thirdPartiesStore.GetValue (iter, 2);
			string email = (string)thirdPartiesStore.GetValue (iter, 3);

			ThirdPartyDialog dlg = new ThirdPartyDialog ();
			dlg.Modal = true;

			dlg.ThirdPartyKey = key;
			dlg.ThirdPartyName = name;
			dlg.ThirdPartyEmail = email;

			int response = dlg.Run ();
			if (response == (int)ResponseType.Ok) {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("third_party[name]", dlg.ThirdPartyName);
				nvc.Add ("third_party[key]", dlg.ThirdPartyKey);
				nvc.Add ("third_party[contact_email]", dlg.ThirdPartyEmail);

				WebServiceClient wsc = new WebServiceClient ();
				Dictionary<string,string> postResponse = wsc.DoPut ("services/third_parties/" + i, nvc);
			}
			dlg.Destroy ();
		}
	}

	protected void OnNewPublicKey (object sender, EventArgs e)
	{
		PublicKeyDialog dlg = new PublicKeyDialog ();
		dlg.Modal = true;

		int response = dlg.Run ();

		if (response == (int)ResponseType.Ok) {
			// Actually save the new service
			string name = dlg.PublicKeyName;
			DateTime validUntil = dlg.PublicKeyValidUntil;
			string keyPath = dlg.PublicKeyFile;

			NameValueCollection nvc = new NameValueCollection ();
			nvc.Add ("public_key[name]", name);
			nvc.Add ("public_key[valid_until]", validUntil.ToString ());

			WebServiceClient wsc = new WebServiceClient ();
			Dictionary<string,string> postResponse = wsc.DoUpload ("services/public_keys", "POST", "public_key[key_file]", keyPath, nvc);
		} 
		dlg.Destroy ();
	}

	protected void OnEditPublicKey (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		// selection.GetSelected(
		if (selection.GetSelected (out iter)) {
			int i = (int)publicKeysStore.GetValue (iter, 0);
			string name = (string)publicKeysStore.GetValue (iter, 1);
			DateTime validUntil = DateTime.Parse((string)publicKeysStore.GetValue (iter, 2));

			PublicKeyDialog dlg = new PublicKeyDialog ();
			dlg.Modal = true;

			dlg.PublicKeyName = name;
			dlg.PublicKeyValidUntil = validUntil;

			int response = dlg.Run ();
			if (response == (int)ResponseType.Ok) {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("public_key[name]", dlg.PublicKeyName);
				nvc.Add ("public_key[valid_until]", dlg.PublicKeyValidUntil.ToString());
				string keyPath = dlg.PublicKeyFile;
				WebServiceClient wsc = new WebServiceClient ();
				Dictionary<string,string> postResponse = wsc.DoUpload ("services/public_keys/" + i, "PUT", "public_key[key_file]", keyPath, nvc);
			}
			dlg.Destroy ();
		}
	}

	protected void OnDeletePublicKey (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		// selection.GetSelected(
		if (selection.GetSelected (out iter)) {
			int i = (int)publicKeysStore.GetValue (iter, 0);
			string name = (string)publicKeysStore.GetValue (iter, 1);
			MessageDialog md = new MessageDialog (
				this, 
				DialogFlags.DestroyWithParent, 
				MessageType.Question, 
				ButtonsType.YesNo, 
				"Are you sure you want to delete public key '{0}'", 
				name);
			int response = md.Run ();
			md.Destroy ();
			if ((int)ResponseType.No == response) {
				return;
			}

			WebServiceClient wsc = new WebServiceClient ();
			Dictionary<string,string> postResponse = wsc.DoDelete ("services/public_keys/" + i, new NameValueCollection());
		}
	}
}
