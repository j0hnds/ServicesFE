using System;

namespace ServicesFE
{
	public class WebServiceConfiguration
	{
		private string name;
		private string baseUri;
		private string token;

		public WebServiceConfiguration ()
		{
		}

		public WebServiceConfiguration(string _name, string _baseUri, string _token)
		{
			name = _name;
			baseUri = _baseUri;
			token = _token;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string BaseUri 
		{
			get { return baseUri; }
			set { baseUri = value; }
		}

		public string Token 
		{
			get { return token; }
			set { token = value; }
		}
	}
}

