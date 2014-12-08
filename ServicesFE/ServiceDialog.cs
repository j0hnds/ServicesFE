using System;
using Gtk;

namespace ServicesFE
{
	public partial class ServiceDialog : Gtk.Dialog
	{
		private Service _service;

		public ServiceDialog ()
		{
			this.Build ();
			buttonOk.Sensitive = false;
		}

		public Service Service
		{
			get {
				return _service;
			}

			set {
				_service = value;
				eServiceName.Text = _service.Name;
				eServiceKey.Text = _service.Key;
				buttonOk.Sensitive = Service.Valid();
			}
		}

		public static void CreateService (System.Action<ServiceDialog> createAction)
		{
			EditService (new Service (), createAction);
		}

		public static void EditService (Service svc, System.Action<ServiceDialog> editAction)
		{
			ServiceDialog dlg = new ServiceDialog ();
			dlg.Modal = true;
			dlg.Service = svc;

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				editAction (dlg);
			} 
			dlg.Destroy ();
		}
			
		protected void OnNameChanged (object sender, EventArgs e)
		{
			Service.Name = eServiceName.Text;
			buttonOk.Sensitive = Service.Valid();
		}

		protected void OnKeyChanged (object sender, EventArgs e)
		{
			Service.Key = eServiceKey.Text;
			buttonOk.Sensitive = Service.Valid();
		}
	}
}

