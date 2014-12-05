using System;
using System.Runtime.Serialization;

namespace ServicesFE
{
	[DataContract]
	public class PublicKey
	{
		public PublicKey ()
		{
		}

		[DataMember(Name="id")]
		public int Id { get; set; }

		[DataMember(Name="name")]
		public string Name { get; set; }

		[DataMember(Name="valid_until",IsRequired=false)]
		public DateTime ValidUntil{ get; set; }

		public string FormattedValidUntil 
		{
			get {
				if (ValidUntil.Year < 1000) {
					return "";
				} else {
					return ValidUntil.ToString ("yyyy-MM-dd HH:mm:ss");
				}
			}
		}
	}
}

