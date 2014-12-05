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
using System.Text.RegularExpressions;

public partial class MainWindow: Gtk.Window
{
	private ListStore tubeStore;
	private ListStore statisticsStore;
	private ListStore servicesStore;
	private ListStore thirdPartiesStore;
	private ListStore publicKeysStore;
	private ListStore sdThirdPartiesStore;
	private ListStore sdServicesStore;
	private ListStore serviceDefinitionsStore;

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

		sdThirdPartiesStore = new ListStore (typeof(string), typeof(int));
		cbThirdParties.Model = sdThirdPartiesStore;

		sdServicesStore = new ListStore (typeof(string), typeof(int));
		cbServices.Model = sdServicesStore;

		SetupServiceDefinitionsTreeView ();

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
		WebClientCall ((wsc) => {
			List<string> tubes = wsc.DoGetList ("beanstalk/tubes");

			tubeStore.Clear ();
			foreach (String tube in tubes) {
				tubeStore.AppendValues (tube);
			}
		});
	}

	protected void OnGetStatistics (object sender, EventArgs e)
	{
		StringBuilder uri = new StringBuilder ("beanstalk");
		// Is there a tube selected?
		string selectedTube = cbTubes.ActiveText;
		if (selectedTube != null) {
			string encodedTube = base64urlencode (Encoding.ASCII.GetBytes (selectedTube));
			uri.AppendFormat ("/tubes/{0}", encodedTube);
		}

		WebClientCall ((wsc) => {
			Dictionary<string,string> statistics = wsc.DoGetDictionary (uri.ToString ());

			statisticsStore.Clear ();
			foreach (string key in statistics.Keys) {
				statisticsStore.AppendValues (key, statistics [key]);
			}
		});
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

			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoPost ("services/services", nvc);
				ShowMessage (postResponse);
			});
		} 
		dlg.Destroy ();
	}

	protected void OnEditService (object sender, EventArgs e)
	{
		TreeSelection selection = servicesTree.Selection;
		TreeIter iter;
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

				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoPut ("services/services/" + i, nvc);
					ShowMessage (postResponse);
				});
			}
			dlg.Destroy ();
		}
	}
		
	protected void LoadServices ()
	{
		servicesStore.Clear ();

		WebClientCall ((wsc) => {
			List<Dictionary<string,string>> services = wsc.DoGetDictionaryList ("services/services");

			foreach (Dictionary<string,string> svc in services) {
				servicesStore.AppendValues (Convert.ToInt32 (svc ["id"]), svc ["name"], svc ["key"]);
			}
		});
	}

	protected void LoadServiceDefinitions (string key, string id)
	{
		serviceDefinitionsStore.Clear ();

		string uri = null;
		if (key == "service_id") {
			uri = String.Format ("services/services/{0}/service_definitions", id);
		} else {
			uri = String.Format ("services/third_parties/{0}/service_definitions", id);
		}
		WebClientCall ((wsc) => {
			List<Dictionary<string,string>> serviceDefinitions = wsc.DoGetDictionaryList (uri);
			foreach (Dictionary<string,string> svcDef in serviceDefinitions) {
				serviceDefinitionsStore.AppendValues (
					Convert.ToInt32 (DictValue (svcDef, "id")), 
					DictValue (svcDef, "hostname"), 
					DictValue (svcDef, "port"), 
					DictValue (svcDef, "base_uri"), 
					DictValue (svcDef, "username"), 
					DictValue (svcDef, "service_class"), 
					DictValue (svcDef, "password"), 
					DictValue (svcDef, "token"),
					Convert.ToInt32 (DictValue (svcDef, "service_id")),
					Convert.ToInt32 (DictValue (svcDef, "third_party_id")));
			}
		});
	}

	private string DictValue(Dictionary<string,string> dict, string key) 
	{
		return dict.ContainsKey (key) ? dict [key] : "";
	}

	protected void LoadServicesCombo ()
	{
		sdServicesStore.Clear ();

		WebClientCall ((wsc) => {
			List<Dictionary<string,string>> services = wsc.DoGetDictionaryList ("services/services");

			foreach (Dictionary<string,string> svc in services) {
				sdServicesStore.AppendValues (svc ["name"], Convert.ToInt32 (svc ["id"]));
			}
		});
	}

	protected void LoadThirdParties ()
	{
		thirdPartiesStore.Clear ();

		WebClientCall ((wsc) => {
			List<Dictionary<string,string>> thirdParties = wsc.DoGetDictionaryList ("services/third_parties");

			foreach (Dictionary<string,string> tp in thirdParties) {
				thirdPartiesStore.AppendValues (Convert.ToInt32 (tp ["id"]), tp ["name"], tp ["key"], tp ["contact_email"]);
			}
		});
	}

	protected void LoadThirdPartiesCombo ()
	{
		sdThirdPartiesStore.Clear ();

		WebClientCall ((wsc) => {
			List<Dictionary<string,string>> thirdParties = wsc.DoGetDictionaryList ("services/third_parties");

			foreach (Dictionary<string,string> tp in thirdParties) {
				sdThirdPartiesStore.AppendValues (tp ["name"], Convert.ToInt32 (tp ["id"]));
			}
		});
	}

	protected void LoadPublicKeys ()
	{
		publicKeysStore.Clear ();

		WebClientCall ((wsc) => {
			List<Dictionary<string,string>> publicKeys = wsc.DoGetDictionaryList ("services/public_keys");

			foreach (Dictionary<string,string> pk in publicKeys) {
				publicKeysStore.AppendValues (Convert.ToInt32 (pk ["id"]), pk ["name"], pk ["valid_until"]);
			}
		});
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

		case 4: // Service Definitions
			LoadThirdPartiesCombo ();
			LoadServicesCombo ();
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

	private void SetupServiceDefinitionsTreeView()
	{
		TreeViewColumn sdHostnameCol = new TreeViewColumn ();
		sdHostnameCol.Title = "Host Name";
		CellRendererText sdHostnameCell = new CellRendererText ();
		sdHostnameCol.PackStart (sdHostnameCell, true);
		sdHostnameCol.AddAttribute (sdHostnameCell, "text", 1);

		TreeViewColumn sdPortCol = new TreeViewColumn ();
		sdPortCol.Title = "Port";
		CellRendererText sdPortCell = new CellRendererText ();
		sdPortCol.PackStart (sdPortCell, true);
		sdPortCol.AddAttribute (sdPortCell, "text", 2);

		TreeViewColumn sdBaseUriCol = new TreeViewColumn ();
		sdBaseUriCol.Title = "Base URI";
		CellRendererText sdBaseUriCell = new CellRendererText ();
		sdBaseUriCol.PackStart (sdBaseUriCell, true);
		sdBaseUriCol.AddAttribute (sdBaseUriCell, "text", 3);

		TreeViewColumn sdUserNameCol = new TreeViewColumn ();
		sdUserNameCol.Title = "User Name";
		CellRendererText sdUserNameCell = new CellRendererText ();
		sdUserNameCol.PackStart (sdUserNameCell, true);
		sdUserNameCol.AddAttribute (sdUserNameCell, "text", 4);

		TreeViewColumn sdServiceClassCol = new TreeViewColumn ();
		sdServiceClassCol.Title = "Service Class";
		CellRendererText sdServiceClassCell = new CellRendererText ();
		sdServiceClassCol.PackStart (sdServiceClassCell, true);
		sdServiceClassCol.AddAttribute (sdServiceClassCell, "text", 5);

		TreeViewColumn sdPasswordCol = new TreeViewColumn ();
		sdPasswordCol.Title = "Password";
		CellRendererText sdPasswordCell = new CellRendererText ();
		sdPasswordCol.PackStart (sdPasswordCell, true);
		sdPasswordCol.AddAttribute (sdPasswordCell, "text", 6);

		TreeViewColumn sdTokenCol = new TreeViewColumn ();
		sdTokenCol.Title = "Token";
		CellRendererText sdTokenCell = new CellRendererText ();
		sdTokenCol.PackStart (sdTokenCell, true);
		sdTokenCol.AddAttribute (sdTokenCell, "text", 7);

		serviceDefinitionsTree.AppendColumn (sdHostnameCol);
		serviceDefinitionsTree.AppendColumn (sdPortCol);
		serviceDefinitionsTree.AppendColumn (sdBaseUriCol);
		serviceDefinitionsTree.AppendColumn (sdUserNameCol);
		serviceDefinitionsTree.AppendColumn (sdServiceClassCol);
		serviceDefinitionsTree.AppendColumn (sdPasswordCol);
		serviceDefinitionsTree.AppendColumn (sdTokenCol);

		serviceDefinitionsStore = new ListStore (typeof(int), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(int), typeof(int));
		serviceDefinitionsTree.Model = serviceDefinitionsStore;
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

			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoPost ("services/third_parties", nvc);
				ShowMessage (postResponse);
			});
		} 
		dlg.Destroy ();
	}

	protected void OnEditThirdParty (object sender, EventArgs e)
	{
		TreeSelection selection = thirdPartiesTree.Selection;
		TreeIter iter;
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

				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoPut ("services/third_parties/" + i, nvc);
					ShowMessage (postResponse);
				});
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

			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoUpload ("services/public_keys", "POST", "public_key[key_file]", keyPath, nvc);
				ShowMessage (postResponse);
			});
		} 
		dlg.Destroy ();
	}

	protected void OnEditPublicKey (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			int i = (int)publicKeysStore.GetValue (iter, 0);
			string name = (string)publicKeysStore.GetValue (iter, 1);
			DateTime validUntil = DateTime.Parse ((string)publicKeysStore.GetValue (iter, 2));

			PublicKeyDialog dlg = new PublicKeyDialog ();
			dlg.Modal = true;

			dlg.PublicKeyName = name;
			dlg.PublicKeyValidUntil = validUntil;

			int response = dlg.Run ();
			if (response == (int)ResponseType.Ok) {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("public_key[name]", dlg.PublicKeyName);
				nvc.Add ("public_key[valid_until]", dlg.PublicKeyValidUntil.ToString ());
				string keyPath = dlg.PublicKeyFile;
				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoUpload ("services/public_keys/" + i, "PUT", "public_key[key_file]", keyPath, nvc);
					ShowMessage (postResponse);
				});
			}
			dlg.Destroy ();
		}
	}

	protected void OnDeletePublicKey (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			int i = (int)publicKeysStore.GetValue (iter, 0);
			string name = (string)publicKeysStore.GetValue (iter, 1);
			ConfirmIt (string.Format ("Are you sure you want to delete public key '{0}'?", 
				name), () => {
				WebClientCall((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoDelete ("services/public_keys/" + i, new NameValueCollection ());
					ShowMessage (postResponse);
				});
			});
		}
	}

	protected void OnNewServiceDefinition (object sender, EventArgs e)
	{
		ServiceDefinitionDialog dlg = new ServiceDefinitionDialog ();
		dlg.Modal = true;
		dlg.Services = sdServicesStore;
		dlg.ThirdParties = sdThirdPartiesStore;

		int response = dlg.Run ();

		if (response == (int)ResponseType.Ok) {
			// Actually save the new service definition
			int serviceId = dlg.ServiceId;
			int thirdPartyId = dlg.ThirdPartyId;
			string hostname = dlg.HostName;
			string port = dlg.Port;
			string baseUri = dlg.BaseURI;
			string userName = dlg.UserName;
			string serviceClass = dlg.ServiceClass;
			string password = dlg.Password;
			string token = dlg.Token;

			NameValueCollection nvc = new NameValueCollection ();
			nvc.Add ("service_definition[service_id]", serviceId.ToString ());
			nvc.Add ("service_definition[third_party_id]", thirdPartyId.ToString ());
			nvc.Add ("service_definition[hostname]", hostname);
			nvc.Add ("service_definition[port]", port);
			nvc.Add ("service_definition[base_uri]", baseUri);
			nvc.Add ("service_definition[username]", userName);
			nvc.Add ("service_definition[service_class]", serviceClass);
			nvc.Add ("credential[password]", password);
			nvc.Add ("credential[token]", token);

			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoPost ("services/third_parties/" + thirdPartyId + "/service_definitions", nvc);
				ShowMessage (postResponse);
			});
		} 
		dlg.Destroy ();
	}

	protected void OnEditServiceDefinition (object sender, EventArgs e)
	{
		TreeSelection selection = serviceDefinitionsTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			int i = (int)serviceDefinitionsStore.GetValue (iter, 0);

			ServiceDefinitionDialog dlg = new ServiceDefinitionDialog ();
			dlg.Modal = true;
			dlg.Services = sdServicesStore;
			dlg.ThirdParties = sdThirdPartiesStore;
			dlg.HostName = (string)serviceDefinitionsStore.GetValue (iter, 1);
			dlg.Port = (string)serviceDefinitionsStore.GetValue (iter, 2);
			dlg.BaseURI = (string)serviceDefinitionsStore.GetValue (iter, 3);
			dlg.UserName = (string)serviceDefinitionsStore.GetValue (iter, 4);
			dlg.ServiceClass = (string)serviceDefinitionsStore.GetValue (iter, 5);
			dlg.Password = (string)serviceDefinitionsStore.GetValue (iter, 6);
			dlg.Token = (string)serviceDefinitionsStore.GetValue (iter, 7);
			dlg.ServiceId = (int)serviceDefinitionsStore.GetValue (iter, 8);
			dlg.ThirdPartyId = (int)serviceDefinitionsStore.GetValue (iter, 9);

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				// Actually save the new service definition
				int serviceId = dlg.ServiceId;
				int thirdPartyId = dlg.ThirdPartyId;
				string hostname = dlg.HostName;
				string port = dlg.Port;
				string baseUri = dlg.BaseURI;
				string userName = dlg.UserName;
				string serviceClass = dlg.ServiceClass;
				string password = dlg.Password;
				string token = dlg.Token;

				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("service_definition[service_id]", serviceId.ToString ());
				nvc.Add ("service_definition[third_party_id]", thirdPartyId.ToString ());
				nvc.Add ("service_definition[hostname]", hostname);
				nvc.Add ("service_definition[port]", port);
				nvc.Add ("service_definition[base_uri]", baseUri);
				nvc.Add ("service_definition[username]", userName);
				nvc.Add ("service_definition[service_class]", serviceClass);
				nvc.Add ("credential[password]", password);
				nvc.Add ("credential[token]", token);

				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoPut ("services/third_parties/" + thirdPartyId + "/service_definitions/" + i, nvc);
					ShowMessage (postResponse);
				});
			} 
			dlg.Destroy ();
		}
	}

	protected void OnDeleteServiceDefinition (object sender, EventArgs e)
	{
		TreeSelection selection = serviceDefinitionsTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			int i = (int)serviceDefinitionsStore.GetValue (iter, 0);
			int thirdPartyId = (int)serviceDefinitionsStore.GetValue (iter, 9);

			string hostname = (string)serviceDefinitionsStore.GetValue (iter, 1);
			ConfirmIt (string.Format ("Are you sure you want to delete service definition '{0}'?", 
				hostname),
				() => {
					WebClientCall((wsc) => {
						Dictionary<string,string> postResponse = wsc.DoDelete ("services/third_parties/" + thirdPartyId + "/service_definitions/" + i, new NameValueCollection ());
						ShowMessage (postResponse);
					});
				});
		}
	}

	protected void OnSdServiceChanged (object sender, EventArgs e)
	{
		// throw new NotImplementedException ();
		// int svcIdx = cbServices.Active;
		TreeIter iter;
		if (cbServices.GetActiveIter (out iter)) {
			int serviceId = (int)sdServicesStore.GetValue (iter, 1);
			LoadServiceDefinitions ("service_id", serviceId.ToString());
		}
	}

	protected void OnSdThirdPartyChanged (object sender, EventArgs e)
	{
		// throw new NotImplementedException ();
		TreeIter iter;
		if (cbThirdParties.GetActiveIter (out iter)) {
			int thirdPartyId = (int)sdThirdPartiesStore.GetValue (iter, 1);
			LoadServiceDefinitions ("third_party_id", thirdPartyId.ToString());
		}
	}

	private string ProcessResponse(Dictionary<string,string> response)
	{
		StringBuilder sb = new StringBuilder ();

		foreach (string key in response.Keys) {
			if (sb.Length > 0) {
				sb.Append (Environment.NewLine);
			}
			sb.AppendFormat ("{0}: {1}", key, response [key]);
		}

		return sb.ToString ();
	}

	private void ShowMessage(Dictionary<string,string> response)
	{
		// Do any of the keys in the response have 'error' in it?
		Regex re = new Regex ("error");
		MessageType mt = MessageType.Info;
		foreach (string key in response.Keys) {
			if (re.IsMatch (key)) {
				mt = MessageType.Error;
				break;
			}
		}

		string rsp = ProcessResponse (response);
		ShowMessage (rsp, mt);
	}

	private void ShowMessage(Exception ex)
	{
		ShowMessage (ex.Message, MessageType.Error);
	}

	private void ShowMessage(string message)
	{
		ShowMessage (message, MessageType.Info);
	}

	private void ShowMessage(string message, MessageType mt)
	{
		MessageDialog md = new MessageDialog (
			                   this, 
			                   DialogFlags.DestroyWithParent, 
			                   mt, 
			                   ButtonsType.Ok, 
			                   message);
		md.Run ();
		md.Destroy ();
	}

	private void ConfirmIt(string message, System.Action confirmedAction)
	{
		MessageDialog md = new MessageDialog (
			this, 
			DialogFlags.DestroyWithParent, 
			MessageType.Question, 
			ButtonsType.YesNo, 
			message);

		int response = md.Run ();
		md.Destroy ();

		if ((int)ResponseType.Yes == response) {
			confirmedAction ();
		}
	}

	private void WebClientCall(System.Action<WebServiceClient> clientAction)
	{
		WebServiceClient wsc = new WebServiceClient ();

		try {
			clientAction(wsc);
		} catch (WebException ex) {
			ShowMessage (ex);
		}
	}
}
