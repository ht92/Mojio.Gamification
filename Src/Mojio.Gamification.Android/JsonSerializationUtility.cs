using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mojio.Gamification.Android
{
	public class JsonSerializationUtility
	{
		public static string Serialize (object theObject)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore };
			return JsonConvert.SerializeObject (theObject, settings);
		}

		public static object Deserialize (string json)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore };
			return JsonConvert.DeserializeObject (json, settings);
		}
	}
}

