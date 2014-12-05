using System;
using System.Runtime.Serialization;

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
	}
}

