using System;
using Gtk;

namespace ServicesFE
{
	public partial class PublicKeyDialog : Gtk.Dialog
	{
		private PublicKey _publicKey;

		public PublicKeyDialog ()
		{
			this.Build ();
		}

		public PublicKey PublicKey
		{
			get {
				return _publicKey;
			}
			set {
				_publicKey = value;
				ePublicKeyName.Text = _publicKey.Name;
				if (_publicKey.ValidUntil.Year < 1000) {
					_publicKey.HasExpirationDate = false;
					chkHasExpirationDate.Active = false;
				} else {
					_publicKey.HasExpirationDate = true;
					chkHasExpirationDate.Active = true;
					calPublicKeyValidUntil.Date = _publicKey.ValidUntil;
				}
				buttonOk.Sensitive = _publicKey.Valid ();
			}
		}

		public static void CreatePublicKey (System.Action<PublicKeyDialog> createAction)
		{
			EditPublicKey (new PublicKey (), createAction);
		}

		public static void EditPublicKey (PublicKey pk, System.Action<PublicKeyDialog> editAction)
		{
			PublicKeyDialog dlg = new PublicKeyDialog ();
			dlg.Modal = true;
			dlg.PublicKey = pk;

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				editAction (dlg);
			} 
			dlg.Destroy ();
		}
			
		public string PublicKeyFile
		{
			get { return fcPublicKeyFile.Filename; }
		}

		protected void OnNameChanged (object sender, EventArgs e)
		{
			_publicKey.Name = ePublicKeyName.Text;
			buttonOk.Sensitive = _publicKey.Valid ();
		}

		protected void OnKeyFileChanged (object sender, EventArgs e)
		{
			_publicKey.KeyFile = fcPublicKeyFile.Filename;
			buttonOk.Sensitive = _publicKey.Valid ();
		}

		protected void OnToggleExpirationDate (object sender, EventArgs e)
		{
			_publicKey.HasExpirationDate = chkHasExpirationDate.Active;
			calPublicKeyValidUntil.Sensitive = chkHasExpirationDate.Active;
			if (_publicKey.HasExpirationDate) {
				_publicKey.ValidUntil = calPublicKeyValidUntil.Date;
			}
		}

		protected void OnDaySelected (object sender, EventArgs e)
		{
			_publicKey.ValidUntil = calPublicKeyValidUntil.Date;
		}
	}
}

