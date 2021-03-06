﻿using System;
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
		ServiceDialog.CreateService ((dlg) => {
			// Actually save the new service
			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoPost ("services/services", dlg.Service.Parameters);
				ShowMessage (postResponse);
			});
			LoadServices();
		});
	}

	protected void OnEditService (object sender, EventArgs e)
	{
		TreeSelection selection = servicesTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			Service svc = (Service)servicesStore.GetValue (iter, 3);

			ServiceDialog.EditService ((Service)servicesStore.GetValue (iter, 3), (dlg) => {
				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoPut ("services/services/" + svc.Id, svc.Parameters);
					ShowMessage (postResponse);
				});
				LoadServices();
			});
		}
	}
		
	protected void LoadServices ()
	{
		servicesStore.Clear ();

		WebClientCall ((wsc) => {
			List<Service> services = wsc.DoGetList<Service> ("services/services");

			foreach (Service svc in services) {
				servicesStore.AppendValues (Convert.ToInt32 (svc.Id), svc.Name, svc.Key, svc);
			}
		});

		buttonEditService.Sensitive = false;
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
			List<ServiceDefinition> serviceDefinitions = wsc.DoGetList<ServiceDefinition> (uri);
			foreach (ServiceDefinition svcDef in serviceDefinitions) {
				serviceDefinitionsStore.AppendValues (
					Convert.ToInt32 (svcDef.Id), 
					HandleEmpty (svcDef.Hostname), 
					HandleEmpty (svcDef.Port), 
					HandleEmpty (svcDef.BaseURI), 
					HandleEmpty (svcDef.Username), 
					HandleEmpty (svcDef.ServiceClass), 
					HandleEmpty (svcDef.Password), 
					HandleEmpty (svcDef.Token),
					Convert.ToInt32 (svcDef.ServiceId),
					Convert.ToInt32 (svcDef.ThirdPartyId),
					svcDef);
			}
		});
	}

	private string HandleEmpty(string value) {
		return (value == null) ? "" : value;
	}

	protected void LoadServicesCombo ()
	{
		sdServicesStore.Clear ();

		WebClientCall ((wsc) => {
			List<Service> services = wsc.DoGetList<Service> ("services/services");

			foreach (Service svc in services) {
				sdServicesStore.AppendValues (svc.Name, Convert.ToInt32 (svc.Id));
			}
		});
	}

	protected void LoadThirdParties ()
	{
		thirdPartiesStore.Clear ();

		WebClientCall ((wsc) => {
			List<ThirdParty> thirdParties = wsc.DoGetList<ThirdParty> ("services/third_parties");

			foreach (ThirdParty tp in thirdParties) {
				thirdPartiesStore.AppendValues (Convert.ToInt32 (tp.Id), tp.Name, tp.Key, tp.ContactEmail, tp);
			}
		});
		buttonEditThirdParty.Sensitive = false;
	}

	protected void LoadThirdPartiesCombo ()
	{
		sdThirdPartiesStore.Clear ();

		WebClientCall ((wsc) => {
			List<ThirdParty> thirdParties = wsc.DoGetList<ThirdParty> ("services/third_parties");

			foreach (ThirdParty tp in thirdParties) {
				sdThirdPartiesStore.AppendValues (tp.Name, Convert.ToInt32 (tp.Id));
			}
		});
	}

	protected void LoadPublicKeys ()
	{
		publicKeysStore.Clear ();

		WebClientCall ((wsc) => {
			List<PublicKey> publicKeys = wsc.DoGetList<PublicKey> ("services/public_keys");

			foreach (PublicKey pk in publicKeys) {
				publicKeysStore.AppendValues (Convert.ToInt32 (pk.Id), pk.Name, pk.FormattedValidUntil, pk);
			}
		});
		buttonEditPublicKey.Sensitive = false;
		buttonDeletePublicKey.Sensitive = false;
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

		servicesStore = new ListStore (typeof(int), 
			typeof(string), 
			typeof(string),
			typeof(Service));

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

		thirdPartiesStore = new ListStore (typeof(int), 
			typeof(string), 
			typeof(string), 
			typeof(string),
			typeof(ThirdParty));
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

		publicKeysStore = new ListStore (typeof(int), 
			typeof(string), 
			typeof(string),
			typeof(PublicKey));
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

		serviceDefinitionsStore = new ListStore (typeof(int), 
			typeof(string), 
			typeof(string), 
			typeof(string), 
			typeof(string), 
			typeof(string), 
			typeof(string), 
			typeof(string), 
			typeof(int), 
			typeof(int),
			typeof(ServiceDefinition));
		serviceDefinitionsTree.Model = serviceDefinitionsStore;
	}

	protected void OnNewThirdParty (object sender, EventArgs e)
	{
		ThirdPartyDialog.CreateThirdParty ((dlg) => {
			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoPost ("services/third_parties", dlg.ThirdParty.Parameters);
				ShowMessage (postResponse);
			});
			LoadThirdParties();
		});
	}

	protected void OnEditThirdParty (object sender, EventArgs e)
	{
		TreeSelection selection = thirdPartiesTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			ThirdParty tp = (ThirdParty)thirdPartiesStore.GetValue (iter, 4);
			ThirdPartyDialog.EditThirdParty (tp, (dlg) => {
				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoPut ("services/third_parties/" + tp.Id, tp.Parameters);
					ShowMessage (postResponse);
				});
				LoadThirdParties();
			});
		}
	}

	protected void OnNewPublicKey (object sender, EventArgs e)
	{
		PublicKeyDialog.CreatePublicKey ((dlg) => {
			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoUpload ("services/public_keys", "POST", "public_key[key_file]", dlg.PublicKeyFile, dlg.PublicKey.Parameters);
				ShowMessage (postResponse);
			});
			LoadPublicKeys();
		});
	}

	protected void OnEditPublicKey (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			PublicKey pk = (PublicKey)publicKeysStore.GetValue (iter, 3);

			PublicKeyDialog.EditPublicKey (pk, (dlg) => {
				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoUpload ("services/public_keys/" + pk.Id, "PUT", "public_key[key_file]", dlg.PublicKeyFile, pk.Parameters);
					ShowMessage (postResponse);
				});
				LoadPublicKeys();
			});
		}
	}

	protected void OnDeletePublicKey (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			PublicKey pk = (PublicKey)publicKeysStore.GetValue (iter, 3);
			ConfirmIt (string.Format ("Are you sure you want to delete public key '{0}'?", 
				pk.Name), () => {
				WebClientCall((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoDelete ("services/public_keys/" + pk.Id, new NameValueCollection ());
					ShowMessage (postResponse);
				});
				LoadPublicKeys();
			});
		}
	}

	protected void OnNewServiceDefinition (object sender, EventArgs e)
	{
		ServiceDefinitionDialog.CreateServiceDefinition (sdServicesStore, sdThirdPartiesStore, (dlg) => {
			WebClientCall ((wsc) => {
				Dictionary<string,string> postResponse = wsc.DoPost ("services/third_parties/" + dlg.ServiceDefinition.ThirdPartyId + "/service_definitions", dlg.ServiceDefinition.Parameters);
				ShowMessage (postResponse);
			});
			LoadServiceDefinitions("third_party_id", dlg.ServiceDefinition.ThirdPartyId.ToString());
		});
	}

	protected void OnEditServiceDefinition (object sender, EventArgs e)
	{
		TreeSelection selection = serviceDefinitionsTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			ServiceDefinition sd = (ServiceDefinition)serviceDefinitionsStore.GetValue (iter, 10);

			ServiceDefinitionDialog.EditServiceDefinition (sdServicesStore, sdThirdPartiesStore, sd, (dlg) => {
				WebClientCall ((wsc) => {
					Dictionary<string,string> postResponse = wsc.DoPut ("services/third_parties/" + sd.ThirdPartyId + "/service_definitions/" + sd.Id, sd.Parameters);
					ShowMessage (postResponse);
				});
				LoadServiceDefinitions("third_party_id", dlg.ServiceDefinition.ThirdPartyId.ToString());
			});
		}
	}

	protected void OnDeleteServiceDefinition (object sender, EventArgs e)
	{
		TreeSelection selection = serviceDefinitionsTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			ServiceDefinition sd = (ServiceDefinition)serviceDefinitionsStore.GetValue (iter, 10);
			ConfirmIt (string.Format ("Are you sure you want to delete service definition '{0}'?", 
				sd.Hostname),
				() => {
					WebClientCall((wsc) => {
						Dictionary<string,string> postResponse = wsc.DoDelete ("services/third_parties/" + sd.ThirdPartyId + "/service_definitions/" + sd.Id, new NameValueCollection ());
						ShowMessage (postResponse);
					});
					LoadServiceDefinitions("third_party_id", sd.ThirdPartyId.ToString());
				});
		}
	}

	protected void OnSdServiceChanged (object sender, EventArgs e)
	{
		TreeIter iter;
		if (cbServices.GetActiveIter (out iter)) {
			int serviceId = (int)sdServicesStore.GetValue (iter, 1);
			LoadServiceDefinitions ("service_id", serviceId.ToString());
		}
	}

	protected void OnSdThirdPartyChanged (object sender, EventArgs e)
	{
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
		} catch (JsonSerializationException ex) {
			ShowMessage (ex);
		}
	}

	protected void OnServiceCursorChanged (object sender, EventArgs e)
	{
		TreeSelection selection = servicesTree.Selection;
		TreeIter iter;
		buttonEditService.Sensitive = selection.GetSelected (out iter);
	}

	protected void OnThirdPartyCursorChanged (object sender, EventArgs e)
	{
		TreeSelection selection = thirdPartiesTree.Selection;
		TreeIter iter;
		buttonEditThirdParty.Sensitive = selection.GetSelected (out iter);
		buttonShowAccountMappings.Sensitive = selection.GetSelected (out iter);
	}

	protected void OnPublicKeyCursorChange (object sender, EventArgs e)
	{
		TreeSelection selection = publicKeysTree.Selection;
		TreeIter iter;
		buttonEditPublicKey.Sensitive = selection.GetSelected (out iter);
		buttonDeletePublicKey.Sensitive = selection.GetSelected (out iter);
	}

	protected void OnServiceDefinitionCursorChanged (object sender, EventArgs e)
	{
		TreeSelection selection = serviceDefinitionsTree.Selection;
		TreeIter iter;
		buttonEditServiceDefinition.Sensitive = selection.GetSelected (out iter);
		buttonDeleteServiceDefinition.Sensitive = selection.GetSelected (out iter);
	}

	protected void OnClickShowAccountMappings (object sender, EventArgs e)
	{

		TreeSelection selection = thirdPartiesTree.Selection;
		TreeIter iter;
		if (selection.GetSelected (out iter)) {
			ThirdParty tp = (ThirdParty)thirdPartiesStore.GetValue (iter, 4);
			AccountMappingsDialog.ManageAccountMappings (tp, (dlg) => {
				// Not doing anything in here...
			});
		}

	}
}
