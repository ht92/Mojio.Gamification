using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

using Mojio;
using Mojio.Client;
using Mojio.Client.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public static class MojioConnectUtility
	{
		public static void Connect()
		{
			Task FetchingDataFromMojio = FetchLatestTrip();
			FetchingDataFromMojio.Wait ();
		}

		public async static Task<Tuple<Trip, List<Event>>> FetchLatestTrip() {
			//------------------Initializing the SDK----------------------//
			Guid appID = new Guid("32059d24-d66e-4071-b59e-0f97f1122c53");
			Guid secretKey = new Guid("929f3903-ede2-46c6-b532-eb39bcd06aa7");
			var mojioId = new Guid ("194e0cb4-9a93-4aed-a3a4-1e51b9cb33d5");

			MojioClient client = new MojioClient(MojioClient.Live); //this is actually connected to the sandbox environment
			var Success =await client.BeginAsync (appID, secretKey); //return true if connected to the server  //takes a while to connect
			//--------------Authenticating a Mojio User------------------//
			if (!client.IsLoggedIn ()) {
				var response = await client.SetUserAsync ("capstoneling", "GamificationofDriving2014");
				var token = response.Data;
//				if (response.StatusCode != HttpStatusCode.OK) {
//					var a = response.ErrorMessage;
//				}
			}

			//setting up mojioID/vehicleID 
			var mojiosReponse = await client.GetAsync<Mojio> ();
			var m2 = mojiosReponse.Data.Data.First(); //access the first simulator which is dummy simulator
			//------Fetching the trips from mojio server--------//

			// Fetch first page of 15 trips
			client.PageSize = 15;
			var results = await client.GetAsync<Trip>();
			//get the most recent trip because we only interested in the last trip.
			Trip mostRecentTrip = results.Data.Data.Last();		
			var TripId = mostRecentTrip.Id;

			// Fetch all events by Trip ID
			MojioResponse<Results<Event>> tripData = await client.GetByAsync <Event,Trip>(TripId);
//			if (tripdata.StatusCode != HttpStatusCode.OK) {
//				var a = tripdata.ErrorMessage;
//			}
			List<Event> tripEvents = tripData.Data.Data.ToList ();
			return Tuple.Create (mostRecentTrip, tripEvents);
		}
	}
}

