using System;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServicesFE
{
	public class WebServiceClient
	{
		private string baseUri;
		private string token;

		public WebServiceClient() : this(WebServiceConfigurations.Instance.CurrentConfiguration ())
		{
		}

		public WebServiceClient(WebServiceConfiguration wsc) : this(wsc.BaseUri, wsc.Token)
		{
		}

		public WebServiceClient (string _baseUri, string _token)
		{
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

			baseUri = _baseUri;
			token = _token;
		}

		public string BaseURI 
		{
			get { return baseUri; }
		}

		public string Token
		{
			get { return token; }
		}

		public byte[] DoGet(string uri) 
		{
			WebClient client = new WebClient ();
			PrepareHeaders (client);

			return client.DownloadData (BuildURL (uri));

		}

		public List<string> DoGetList(string uri)
		{
			byte[] data = DoGet (uri);

			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (List<string>)serializer.Deserialize (reader, typeof(List<string>));
		}

		public Dictionary<string,string> DoGetDictionary(string uri)
		{
			byte[] data = DoGet (uri);

			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (Dictionary<string, string>)serializer.Deserialize (reader, typeof(Dictionary<string,string>));
		}

		protected void PrepareHeaders(WebClient client)
		{
			client.Headers ["Content-type"] = "application/json";
			client.Headers ["Authorization"] = "Token token=\"" + token + "\"";
		}

		protected string BuildURL(string uri)
		{
			return baseUri + "/" + uri;
		}

		protected JsonReader BuildJsonReader(byte[] data)
		{
			String s = Encoding.ASCII.GetString (data);

			StringReader treader = new StringReader (s);
			return new JsonTextReader (treader);
		}
	}
}

