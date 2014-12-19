using System;
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace ServicesFE
{
	[DataContract]
	public class AccountMapping
	{
		public AccountMapping ()
		{
		}

		[DataMember(Name="id")]
		public int Id { get; set; }

		[DataMember(Name="third_party_id")]
		public int ThirdPartyId { get; set; }

		[DataMember(Name="third_party_name")]
		public string ThirdPartyName { get; set; }

		[DataMember(Name="account_id")]
		public int AccountId { get; set; }

		[DataMember(Name="account_name")]
		public string AccountName { get; set; }

		[DataMember(Name="account_code")]
		public string AccountCode { get; set; }

		public NameValueCollection Parameters 
		{
			get {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("account_mapping[account_id]", AccountId.ToString());
				nvc.Add ("account_mapping[account_code]", AccountCode);
				return nvc;
			}
		}

		public bool Valid() 
		{
			bool valid = true;
//			if (Name == null || (Name.Length == 0 || Name.Length > 255)) {
//				return false;
//			}
//			if (Key == null || (Key.Length == 0 || Key.Length > 255)) {
//				return false;
//			}
			return valid;
		}

	}
}

