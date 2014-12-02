using System;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

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

		public Dictionary<string,string>  DoPost(string uri, NameValueCollection parameters)
		{
			WebClient client = new WebClient();
			PrepareHeaders(client, true);

			byte[] data = client.UploadValues(BuildURL(uri), parameters);
			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (Dictionary<string, string>)serializer.Deserialize (reader, typeof(Dictionary<string,string>));
		}

		public Dictionary<string,string>  DoUpload(string uri, string method, string name, string filePath, NameValueCollection parameters)
		{
			WebClient client = new WebClient();
			PrepareHeaders(client, true);

			UploadFile uf = new UploadFile ();
			uf.ContentType = "text/plain";
			uf.Filename = Path.GetFileName (filePath);
			uf.Name = name;
			uf.Stream = File.Open (filePath, FileMode.Open);

			byte[] data = UploadFile (BuildURL (uri), method, uf, parameters);
			// byte[] data = client.UploadValues(BuildURL(uri), parameters);
			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (Dictionary<string, string>)serializer.Deserialize (reader, typeof(Dictionary<string,string>));
		}

		public Dictionary<string,string>  DoPut(string uri, NameValueCollection parameters)
		{
			WebClient client = new WebClient();
			PrepareHeaders(client, true);

			byte[] data = client.UploadValues(BuildURL(uri), "PUT", parameters);
			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (Dictionary<string, string>)serializer.Deserialize (reader, typeof(Dictionary<string,string>));
		}

		public Dictionary<string,string>  DoDelete(string uri, NameValueCollection parameters)
		{
			WebClient client = new WebClient();
			PrepareHeaders(client, true);

			byte[] data = client.UploadValues(BuildURL(uri), "DELETE", parameters);
			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (Dictionary<string, string>)serializer.Deserialize (reader, typeof(Dictionary<string,string>));
		}

		public List<string> DoGetList(string uri)
		{
			byte[] data = DoGet (uri);

			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (List<string>)serializer.Deserialize (reader, typeof(List<string>));
		}

		public List<Dictionary<string,string>> DoGetDictionaryList(string uri)
		{
			byte[] data = DoGet (uri);

			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (List<Dictionary<string,string>>)serializer.Deserialize (reader, typeof(List<Dictionary<string,string>>));
		}

		public Dictionary<string,string> DoGetDictionary(string uri)
		{
			byte[] data = DoGet (uri);

			JsonReader reader = BuildJsonReader (data);

			JsonSerializer serializer = new JsonSerializer ();
			return (Dictionary<string, string>)serializer.Deserialize (reader, typeof(Dictionary<string,string>));
		}

		protected void PrepareHeaders(WebClient client, bool postRequest=false)
		{
			if (!postRequest) {
				client.Headers ["Content-Type"] = "application/json";
			}
			client.Headers ["Accept"] = "application/json";
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

		protected byte[] UploadFile(string address, string method, UploadFile file, NameValueCollection parameters)
		{
			var request = WebRequest.Create(address);
			// request.Headers ["Accept"] = "application/json";
			request.Headers ["Authorization"] = "Token token=\"" + token + "\"";
			request.Method = method;
			var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
			request.ContentType = "multipart/form-data; boundary=" + boundary;

			boundary = "--" + boundary;

			using (var requestStream = request.GetRequestStream())
			{
				// Write the values
				foreach (string name in parameters.Keys)
				{
					var buffer = Encoding.ASCII.GetBytes (boundary + "\r\n"); //Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);
					buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, "\r\n")); //Environment.NewLine));
					requestStream.Write(buffer, 0, buffer.Length);
					buffer = Encoding.UTF8.GetBytes (parameters [name] + "\r\n"); // Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);
				}

				{
					var buffer = Encoding.ASCII.GetBytes (boundary + "\r\n"); // Environment.NewLine);
					requestStream.Write (buffer, 0, buffer.Length);
					buffer = Encoding.UTF8.GetBytes (string.Format ("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, "\r\n")); // Environment.NewLine));
					requestStream.Write (buffer, 0, buffer.Length);
					buffer = Encoding.ASCII.GetBytes (string.Format ("Content-Type: {0}{1}{1}", file.ContentType, "\r\n")); // Environment.NewLine));
					requestStream.Write (buffer, 0, buffer.Length);
					file.Stream.CopyTo (requestStream);
					buffer = Encoding.ASCII.GetBytes ("\r\n"); // Environment.NewLine);
					requestStream.Write (buffer, 0, buffer.Length);
				}
				var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
				requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
			}

			using (var response = request.GetResponse())
			using (var responseStream = response.GetResponseStream())
			using (var stream = new MemoryStream())
			{
				responseStream.CopyTo(stream);
				return stream.ToArray();
			}
		}
		
	}
}

