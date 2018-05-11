using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SouthPointe.Serialization.MessagePack
{
	public class CamelCaseNamingStrategy : IMapNamingStrategy
	{
		TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

		public string OnPack(string name, MapDefinition definition)
		{
			return char.ToLowerInvariant(name[0]) + name.Substring(1); 
		}

		public string OnUnpack(string name, MapDefinition definition)
		{
			return char.ToUpperInvariant(name[0]) + name.Substring(1);
		}
	}
}
