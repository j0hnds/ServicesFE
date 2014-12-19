using System;
using Gtk;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServicesFE
{
	public partial class AccountMappingsDialog : Gtk.Dialog
	{
		private ThirdParty _thirdParty;

		private ListStore _accountMappingsStore;

		public AccountMappingsDialog ()
		{
			this.Build ();

			SetUpAccountMappingsTree ();
		}

		public ThirdParty ThirdParty
		{
			get { return _thirdParty; }
			set { 
				_thirdParty = value; 
				LoadAccountMappings ();
			}
		}

		public static void ManageAccountMappings (ThirdParty tp, System.Action<AccountMappingsDialog> manageAction)
		{
			AccountMappingsDialog dlg = new AccountMappingsDialog ();
			dlg.Modal = true;
			dlg.ThirdParty = tp;

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				manageAction (dlg);
			} 
			dlg.Destroy ();
		}

		private void SetUpAccountMappingsTree() 
		{
			TreeViewColumn amAccountName = new TreeViewColumn ();
			amAccountName.Title = "Account";
			CellRendererText amAccountNameCell = new CellRendererText ();
			amAccountName.PackStart (amAccountNameCell, true);
			amAccountName.AddAttribute (amAccountNameCell, "text", 1);

			TreeViewColumn amThirdPartyCode = new TreeViewColumn ();
			amThirdPartyCode.Title = "TP Account Code";
			CellRendererText amThirdPartyCodeCell = new CellRendererText ();
			amThirdPartyCode.PackStart (amThirdPartyCodeCell, true);
			amThirdPartyCode.AddAttribute (amThirdPartyCodeCell, "text", 2);

			tvAccountMappings.AppendColumn (amAccountName);
			tvAccountMappings.AppendColumn (amThirdPartyCode);

			_accountMappingsStore = new ListStore (typeof(int), 
				typeof(string), 
				typeof(string),
				typeof(ThirdParty));

			tvAccountMappings.Model = _accountMappingsStore;

		}

		private void LoadAccountMappings ()
		{
			_accountMappingsStore.Clear ();

			WebClientCall ((wsc) => {
				List<AccountMapping> accountMappings = wsc.DoGetList<AccountMapping> ("services/third_parties/" + ThirdParty.Id + "/account_mappings");

				foreach (AccountMapping am in accountMappings) {
					_accountMappingsStore.AppendValues (Convert.ToInt32 (am.Id), am.AccountName, am.AccountCode, am);
				}
			});

			// buttonEditService.Sensitive = false;
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

	}
}

