using System;
using System.Runtime.Serialization;

namespace ServicesFE
{
	[DataContract]
	public class ServiceDefinition
	{
		public ServiceDefinition ()
		{
		}

		[DataMember(Name="Id")]
		public int Id { get; set; }

		[DataMember(Name="service_id")]
		public int ServiceId { get; set; }

		[DataMember(Name="third_party_id")]
		public int ThirdPartyId { get; set; }

		[DataMember(Name="hostname")]
		public string Hostname { get; set; }

		[DataMember(Name="port")]
		public string Port { get; set; }

		[DataMember(Name="base_uri")]
		public string BaseURI { get; set; }

		[DataMember(Name="username")]
		public string Username { get; set; }

		[DataMember(Name="service_class")]
		public string ServiceClass { get; set; }

		[DataMember(Name="password")]
		public string Password { get; set; }

		[DataMember(Name="token")]
		public string Token { get; set; }
	}
}

