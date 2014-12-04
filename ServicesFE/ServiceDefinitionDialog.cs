using System;
using Gtk;
using System.Web.Security;

namespace ServicesFE
{
	public partial class ServiceDefinitionDialog : Gtk.Dialog
	{
		public ServiceDefinitionDialog ()
		{
			this.Build ();
		}

		public ListStore Services 
		{
			set { cbServices.Model = value; }
		}

		public int ActiveService
		{
			get { return cbServices.Active; }
			set { cbServices.Active = value; }
		}

		public int ActiveThirdParty
		{
			get { return cbThirdParties.Active; }
			set { cbThirdParties.Active = value; }
		}

		public int ServiceId 
		{
			get {
				int id = -1;
				TreeIter iter;
				if (cbServices.GetActiveIter (out iter)) {
					id = (int)cbServices.Model.GetValue (iter, 1);
				}
				return id;
			}
			set {
				cbServices.Active = ActiveIndex ((ListStore) cbServices.Model, value);
			}
		}

		public int ThirdPartyId 
		{
			get {
				int id = -1;
				TreeIter iter;
				if (cbThirdParties.GetActiveIter (out iter)) {
					id = (int)cbThirdParties.Model.GetValue (iter, 1);
				}
				return id;
			}
			set {
				cbThirdParties.Active = ActiveIndex ((ListStore) cbThirdParties.Model, value);
			}
		}

		public ListStore ThirdParties
		{
			set { cbThirdParties.Model = value; }
		}

//		public string ServiceName
//		{
//			set { lblService.Text = value; }
//		}
//
//		public string ThirdPartyName
//		{
//			set { lblThirdParty.Text = value; }
//		}

		public string HostName
		{
			get { return eHostName.Text; }
			set { eHostName.Text = value; }
		}

		public string Port
		{
			get { return ePort.Text; }
			set { ePort.Text = value; }
		}

		public string BaseURI
		{
			get { return eBaseUri.Text; }
			set { eBaseUri.Text = value; }
		}

		public string UserName
		{
			get { return eUserName.Text; }
			set { eUserName.Text = value; }
		}

		public string ServiceClass
		{
			get { return eServiceClass.Text; }
			set { eServiceClass.Text = value; }
		}

		public string Password
		{
			get { return ePassword.Text; }
			set { ePassword.Text = value; }
		}

		public string Token
		{
			get { return eToken.Text; }
			set { eToken.Text = value; }
		}

		private int ActiveIndex(ListStore store, int selectedId)
		{
			TreeIter iter;
			int foundIndex = -1;
			int index = 0;
			bool found = store.GetIterFirst (out iter);
			while (found) {
				int id = (int)store.GetValue (iter, 1);
				if (id == selectedId) {
					foundIndex = index;
					break;
				}
				index++;
				found = store.IterNext (ref iter);
			}

			return foundIndex;
		}

		protected void OnGeneratePassword (object sender, EventArgs e)
		{
			ePassword.Text = Membership.GeneratePassword(12, 3);
		}

		protected void OnGenerateToken (object sender, EventArgs e)
		{
			eToken.Text = Guid.NewGuid ().ToString ("d").Substring (1, 8);
		}
	}
}

