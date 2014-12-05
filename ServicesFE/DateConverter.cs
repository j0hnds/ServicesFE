using System;
using Newtonsoft.Json;
using System.Globalization;

namespace ServicesFE
{
	public class DateConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Object dt = reader.Value;
			return (dt == null) ? new DateTime () : dt;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}
	}
}

