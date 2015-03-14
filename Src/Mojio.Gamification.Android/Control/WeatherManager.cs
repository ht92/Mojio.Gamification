using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;

using Android.Locations;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public static class WeatherManager
	{
		private static int REQ_TIMEOUT = 10000;

		public enum WeatherType 
		{ 
			SUNNY,
			CLOUDY,
			RAINING,
			SNOWING
		}

		public static WeatherType GetWeather (global::Android.Locations.Location location)
		{
			try {
				String url = String.Format ("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}", location.Latitude, location.Longitude);
				HttpWebRequest request = (HttpWebRequest) WebRequest.Create (url);

				request.KeepAlive = false;
				request.Method = "GET";
				request.Timeout = REQ_TIMEOUT;

				HttpWebResponse response = (HttpWebResponse) request.GetResponse ();
				using (StreamReader reader = new StreamReader (response.GetResponseStream ())) {
					string json = reader.ReadToEnd ();
					var parsedResponse = Newtonsoft.Json.Linq.JObject.Parse (json);
					string weather = (string) parsedResponse ["weather"][0]["main"];
					return getWeatherType(weather);
				}
			} catch (Exception e) {
				Logger.GetInstance ().Warning ("Unable to get weather. Returning default weather.");
				return WeatherType.SUNNY;
			}
		}

		private static WeatherType getWeatherType (string weather)
		{
			weather = weather.ToLower ();
			switch (weather) {
			case "clouds":
				return WeatherType.CLOUDY;
			case "rain":
			case "drizzle":
			case "thunderstorm":
			case "extreme":
				return WeatherType.RAINING;
			case "snow":
				return WeatherType.SNOWING;
			default:
				return WeatherType.SUNNY;
			}
		}
	}
}

