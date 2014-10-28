using System;
using System.Threading.Tasks;

using Mojio;
using Mojio.Client;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public static class MojioConnectUtility
	{
//		public static MojioClient Connect(bool isLive, Guid appId, Guid secretKey) 
//		{
//			string endPoint = isLive ? MojioClient.Live : MojioClient.Sandbox;
//			MojioClient client = new MojioClient (endPoint);
//			bool success = await client.BeginAsync (appId, secretKey);
//			return client;
//		}
//
//		public static Results<Trip> GetTrips(MojioClient client, int numPages = 15) 
//		{
//			client.PageSize = numPages;
//			Task<MojioResponse<Results<Trip>>> task = client.GetAsync<Trip> ();
//			MojioResponse<Results<Trip>> response = await task;
//			Results<Trip> results = response.Data;
//			return results;
//		}

		public static void GetEvents(MojioClient client)
		{
			//string mojioId = "blah";
			//Results<Events> events = client.GetByAsync<Event,Device> (mojioId);

		}
	}
}

