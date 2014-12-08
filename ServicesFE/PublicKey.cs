using System;
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace ServicesFE
{
	[DataContract]
	public class PublicKey
	{
		public PublicKey ()
		{
		}

		[DataMember(Name="id")]
		public int Id { get; set; }

		[DataMember(Name="name")]
		public string Name { get; set; }

		[DataMember(Name="valid_until",IsRequired=false)]
		public DateTime ValidUntil{ get; set; }

		public string KeyFile { get; set; }

		public bool HasExpirationDate { get; set; }

		public string FormattedValidUntil 
		{
			get {
				if (ValidUntil.Year < 1000) {
					return "";
				} else {
					return ValidUntil.ToString ("yyyy-MM-dd HH:mm:ss");
				}
			}
		}

		public NameValueCollection Parameters 
		{
			get {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("public_key[name]", Name);
				if (HasExpirationDate) {
					nvc.Add ("public_key[valid_until]", ValidUntil.ToString ("yyyy-MM-dd HH:mm:ss"));
				}
				return nvc;
			}
		}

		public bool Valid() 
		{
			bool valid = true;
			if (Name == null || (Name.Length == 0 || Name.Length > 255)) {
				return false;
			}
			if (KeyFile == null || (KeyFile.Length == 0 || KeyFile.Length > 255)) {
				return false;
			}
			return valid;
		}

	}
}

