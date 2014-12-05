using System;
using Gtk;

namespace ServicesFE
{
	public partial class ServiceDialog : Gtk.Dialog
	{
		public ServiceDialog ()
		{
			this.Build ();
		}

		public string ServiceName {
			get { return eServiceName.Text; }
			set { eServiceName.Text = value; }
		}

		public string ServiceKey {
			get { return eServiceKey.Text; }
			set { eServiceKey.Text = value; }
		}

		public static void CreateService (System.Action<ServiceDialog> createAction)
		{
			ServiceDialog dlg = new ServiceDialog ();
			dlg.Modal = true;

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				createAction (dlg);
			} 
			dlg.Destroy ();
		}
			
	}
}

