using System;
using Gtk;
using System.Web.Security;

namespace ServicesFE
{
	public partial class ServiceDefinitionDialog : Gtk.Dialog
	{
		private ServiceDefinition _serviceDefinition;

		public ServiceDefinitionDialog ()
		{
			this.Build ();
		}

		public ListStore Services 
		{
			set { cbServices.Model = value; }
		}

		public ServiceDefinition ServiceDefinition
		{
			get {
				return _serviceDefinition;
			}
			set {
				_serviceDefinition = value;
				if (_serviceDefinition.ServiceId > 0) {
					cbServices.Active = ActiveIndex ((ListStore)cbServices.Model, _serviceDefinition.ServiceId);
					cbServices.Sensitive = false;
				}
				if (_serviceDefinition.ThirdPartyId > 0) {
					cbThirdParties.Active = ActiveIndex ((ListStore)cbThirdParties.Model, _serviceDefinition.ThirdPartyId);
					cbThirdParties.Sensitive = false;
				}
				eHostName.Text = _serviceDefinition.Hostname;
				ePort.Text = _serviceDefinition.Port;
				eBaseUri.Text = _serviceDefinition.BaseURI;
				eUserName.Text = _serviceDefinition.Username;
				eServiceClass.Text = _serviceDefinition.ServiceClass;
				ePassword.Text = _serviceDefinition.Password;
				eToken.Text = _serviceDefinition.Token;
			}
		}

		public static void CreateServiceDefinition (ListStore servicesStore, ListStore thirdPartiesStore, System.Action<ServiceDefinitionDialog> createAction)
		{
			EditServiceDefinition (servicesStore, thirdPartiesStore, new ServiceDefinition (), createAction);
		}

		public static void EditServiceDefinition (ListStore servicesStore, ListStore thirdPartiesStore, ServiceDefinition svc, System.Action<ServiceDefinitionDialog> editAction)
		{
			ServiceDefinitionDialog dlg = new ServiceDefinitionDialog ();
			dlg.Modal = true;
			dlg.Services = servicesStore;
			dlg.ThirdParties = thirdPartiesStore;
			dlg.ServiceDefinition = svc;

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				editAction (dlg);
			} 
			dlg.Destroy ();
		}

		public ListStore ThirdParties
		{
			set { cbThirdParties.Model = value; }
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
			_serviceDefinition.Password = Membership.GeneratePassword(12, 3);
			ePassword.Text = _serviceDefinition.Password;
		}

		protected void OnGenerateToken (object sender, EventArgs e)
		{
			_serviceDefinition.Token = Guid.NewGuid ().ToString ("d").Substring (1, 8);
			eToken.Text = _serviceDefinition.Token;
		}

		protected void OnServiceChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			if (cbServices.GetActiveIter (out iter)) {
				_serviceDefinition.ServiceId = (int)cbServices.Model.GetValue (iter, 1);
			}

		}

		protected void OnThirdPartyChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			if (cbThirdParties.GetActiveIter (out iter)) {
				_serviceDefinition.ThirdPartyId = (int)cbThirdParties.Model.GetValue (iter, 1);
			}
		}

		protected void OnHostnameChanged (object sender, EventArgs e)
		{
			_serviceDefinition.Hostname = eHostName.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}

		protected void OnPortChanged (object sender, EventArgs e)
		{
			_serviceDefinition.Port = ePort.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}

		protected void OnBaseUriChanged (object sender, EventArgs e)
		{
			_serviceDefinition.BaseURI = eBaseUri.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}

		protected void OnUserNameChanged (object sender, EventArgs e)
		{
			_serviceDefinition.Username = eUserName.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}

		protected void OnServiceClassChanged (object sender, EventArgs e)
		{
			_serviceDefinition.ServiceClass = eServiceClass.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}

		protected void OnPasswordChanged (object sender, EventArgs e)
		{
			_serviceDefinition.Password = ePassword.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}

		protected void OnTokenChanged (object sender, EventArgs e)
		{
			_serviceDefinition.Token = eToken.Text;
			buttonOk.Sensitive = _serviceDefinition.Valid ();
		}
	}
}

