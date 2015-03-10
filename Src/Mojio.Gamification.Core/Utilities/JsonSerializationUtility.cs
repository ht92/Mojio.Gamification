using Newtonsoft.Json;

namespace Mojio.Gamification.Core
{
	public class JsonSerializationUtility
	{
		public static string Serialize (object theObject)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore, FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.Symbol };
			return JsonConvert.SerializeObject (theObject, settings);
		}

		public static object Deserialize (string json)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore, FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.Symbol };
			return JsonConvert.DeserializeObject (json, settings);
		}
	}
}

