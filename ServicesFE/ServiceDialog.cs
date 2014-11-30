using System;

namespace ServicesFE
{
	public partial class ServiceDialog : Gtk.Dialog
	{
		public ServiceDialog ()
		{
			this.Build ();
		}

		public string ServiceName
		{
			get { return eServiceName.Text; }
			set { eServiceName.Text = value; }
		}

		public string ServiceKey
		{
			get { return eServiceKey.Text; }
			set { eServiceKey.Text = value; }
		}

//		protected void OnOK (object sender, EventArgs e)
//		{
//			throw new NotImplementedException ();
//		}
//
//		protected void OnCancel (object sender, EventArgs e)
//		{
//			throw new NotImplementedException ();
//		}
	}
}

