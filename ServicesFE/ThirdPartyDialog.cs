using System;
using Gtk;

namespace ServicesFE
{
	public partial class ThirdPartyDialog : Gtk.Dialog
	{
		private ThirdParty _thirdParty;

		public ThirdPartyDialog ()
		{
			this.Build ();
			buttonOk.Sensitive = false;
		}

		public ThirdParty ThirdParty
		{
			get { 
				return _thirdParty; 
			}
			set {
				_thirdParty = value;
				eThirdPartyName.Text = _thirdParty.Name;
				eThirdPartyKey.Text = _thirdParty.Key; 
				eThirdPartyEmail.Text = _thirdParty.ContactEmail;
				buttonOk.Sensitive = _thirdParty.Valid ();
			}
		}

		public static void CreateThirdParty (System.Action<ThirdPartyDialog> createAction)
		{
			EditThirdParty (new ThirdParty (), createAction);
		}

		public static void EditThirdParty (ThirdParty tp, System.Action<ThirdPartyDialog> editAction)
		{
			ThirdPartyDialog dlg = new ThirdPartyDialog ();
			dlg.Modal = true;
			dlg.ThirdParty = tp;

			int response = dlg.Run ();

			if (response == (int)ResponseType.Ok) {
				editAction (dlg);
			} 
			dlg.Destroy ();
		}

		protected void OnNameChanged (object sender, EventArgs e)
		{
			ThirdParty.Name = eThirdPartyName.Text;
			buttonOk.Sensitive = ThirdParty.Valid ();
		}

		protected void OnKeyChanged (object sender, EventArgs e)
		{
			ThirdParty.Key = eThirdPartyKey.Text;
			buttonOk.Sensitive = ThirdParty.Valid ();
		}

		protected void OnContactEmailChanged (object sender, EventArgs e)
		{
			ThirdParty.ContactEmail = eThirdPartyEmail.Text;
			buttonOk.Sensitive = ThirdParty.Valid ();
		}
	}
}

