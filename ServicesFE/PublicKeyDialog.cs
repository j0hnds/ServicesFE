using System;

namespace ServicesFE
{
	public partial class PublicKeyDialog : Gtk.Dialog
	{
		public PublicKeyDialog ()
		{
			this.Build ();
		}

		public string PublicKeyName
		{
			get { return ePublicKeyName.Text; }
			set { ePublicKeyName.Text = value; }
		}

		public DateTime PublicKeyValidUntil
		{
			get { return calPublicKeyValidUntil.Date; }
			set { calPublicKeyValidUntil.Date = value; }
		}

		public string PublicKeyFile
		{
			get { return fcPublicKeyFile.Filename; }
		}
	}
}

