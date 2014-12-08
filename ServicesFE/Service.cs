using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace ServicesFE
{
	[DataContract]
	public class Service
	{
		public Service ()
		{
		}

		[DataMember(Name="id")]
		public int Id { get; set; }

		[DataMember(Name="name")]
		public string Name { get; set; }

		[DataMember(Name="key")]
		public string Key { get; set; }

		public NameValueCollection Parameters 
		{
			get {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("service[name]", Name);
				nvc.Add ("service[key]", Key);
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
			return valid;
		}
	}
}

