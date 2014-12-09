using System;
using System.Runtime.Serialization;
using System.Collections.Specialized;

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

		public NameValueCollection Parameters 
		{
			get {
				NameValueCollection nvc = new NameValueCollection ();
				nvc.Add ("service_definition[service_id]", ServiceId.ToString ());
				nvc.Add ("service_definition[third_party_id]", ThirdPartyId.ToString ());
				nvc.Add ("service_definition[hostname]", Hostname);
				if (Port != null) {
					nvc.Add ("service_definition[port]", Port);
				}
				nvc.Add ("service_definition[base_uri]", BaseURI);
				if (Username != null) {
					nvc.Add ("service_definition[username]", Username);
				}
				nvc.Add ("service_definition[service_class]", ServiceClass);
				if (Password != null) {
					nvc.Add ("credential[password]", Password);
				}
				if (Token != null) {
					nvc.Add ("credential[token]", Token);
				}
				return nvc;
			}
		}

		public bool Valid() 
		{
			bool valid = true;
			if (Hostname == null || (Hostname.Length == 0 || Hostname.Length > 255)) {
				return false;
			}
			if (BaseURI == null || (BaseURI.Length == 0 || BaseURI.Length > 255)) {
				return false;
			}
			if (ServiceClass == null || (ServiceClass.Length == 0 || ServiceClass.Length > 255)) {
				return false;
			}
			return valid;
		}

	}
}

