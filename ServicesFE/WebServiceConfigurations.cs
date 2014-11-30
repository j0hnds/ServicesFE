using System;
using System.Collections.Generic;

namespace ServicesFE
{
	public class WebServiceConfigurations
	{
		private static readonly WebServiceConfigurations instance = new WebServiceConfigurations();

		private List<WebServiceConfiguration> configurations;
		private int currentConfiguration;

		public static WebServiceConfigurations Instance
		{
			get { return instance; }
		}

		private WebServiceConfigurations ()
		{
			currentConfiguration = -1;
			configurations = new List<WebServiceConfiguration> ();
		}

		public WebServiceConfiguration CurrentConfiguration()
		{
			return currentConfiguration >= 0 ? configurations [currentConfiguration] : null;
		}

		public void SetCurrentConfiguration(string name)
		{
			currentConfiguration = configurations.FindIndex (wsc => wsc.Name == name); 
		}

		public void Add(WebServiceConfiguration wsc)
		{
			configurations.Add (wsc);
			if (configurations.Count == 1) {
				// If this is the first configuration, then it should
				// be the current configuration.
				currentConfiguration = 0;
			}
		}
	}
}

