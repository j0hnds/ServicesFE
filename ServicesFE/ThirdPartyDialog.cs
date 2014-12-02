using System;

namespace ServicesFE
{
	public partial class ThirdPartyDialog : Gtk.Dialog
	{
		public ThirdPartyDialog ()
		{
			this.Build ();
		}

		public string ThirdPartyName
		{
			get { return eThirdPartyName.Text; }
			set { eThirdPartyName.Text = value; }
		}

		public string ThirdPartyKey
		{
			get { return eThirdPartyKey.Text; }
			set { eThirdPartyKey.Text = value; }
		}

		public string ThirdPartyEmail
		{
			get { return eThirdPartyEmail.Text; }
			set { eThirdPartyEmail.Text = value; }
		}
	}
}

