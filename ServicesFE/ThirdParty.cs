using System;
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace ServicesFE
{
	[DataContract]
	public class ThirdParty
	{
		public ThirdParty ()
		{
		}

		[DataMember(Name="id")]
		public int Id { get; set; }

		[DataMember(Name="name")]
		public string Name { get; set; }

		[DataMember(Name="key")]
		public string Key { get; set; }

		[DataMember(Name="contact_email")]
		public string ContactEmail { get; set; }

		public NameValueCollection Parameters 
		{
			get {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("third_party[name]", Name);
				nvc.Add ("third_party[key]", Key);
				nvc.Add ("third_party[contact_email]", ContactEmail);
				return nvc;
			}
		}

		public bool Valid() 
		{
			bool valid = true;
			if (Name == null || (Name.Length == 0 || Name.Length > 255)) {
				return false;
			}
			if (Key == null || (Key.Length == 0 || Key.Length > 255)) {
				return false;
			}
			if (ContactEmail == null || (ContactEmail.Length == 0 || ContactEmail.Length > 255)) {
				return false;
			}
			return valid;
		}
	}
}

