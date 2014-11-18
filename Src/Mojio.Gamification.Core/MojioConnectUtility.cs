using System;
using System.Linq;
using System.Net;
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
			Task FetchingDataFromMojio = DoWork();
			FetchingDataFromMojio.Wait ();
		}


		public async static Task DoWork() {
			//------------------Initializing the SDK----------------------//
			Guid appID = new Guid("32059d24-d66e-4071-b59e-0f97f1122c53");
			Guid secretKey = new Guid("929f3903-ede2-46c6-b532-eb39bcd06aa7");
			//Guid tokenID = new Guid ("0a5453a0-7e70-16d1-a2w6-28dl98c10200");    how to get token id?
			var mojioId = new Guid ("194e0cb4-9a93-4aed-a3a4-1e51b9cb33d5");

			//Begin a new session using the App ID and secretKey. 
			MojioClient mojioClient = new MojioClient(MojioClient.Live); //this is actually connected to the sandbox environment
			var Success = await mojioClient.BeginAsync (appID, secretKey); //return true if connected to the server  //takes a while to connect

			//--------------Authenticating a Mojio User------------------//
			if (!mojioClient.IsLoggedIn ()) {
				var response = await mojioClient.SetUserAsync (/*userName*/"capstoneling", /*password*/"GamificationofDriving2014");
				//setting up the tokenID
				var token = response.Data;
				//check for status code..if status code is "OK" then proceed otherwise store error message in a and possibly display a to the output
				//this is to ensure the user and password is correct
				//if (response.StatusCode != HttpStatusCode.OK) {
				//	var a = response.ErrorMessage;
				//}
			}

			//setting up mojioID/vehicleID 
			//come after log in and authentication
			var mojiosReponse = await mojioClient.GetAsync<Mojio> ();
			var m2 = mojiosReponse.Data.Data.First(); //access the first simulator which is dummy simulator

			//start getting events
			var fetchEventsQuery = (from e in mojioClient.Queryable<Event> ()
				//where e.EventType.Equals (EventType.HardLeft) || e.EventType.Equals (EventType.HardRight) || e.EventType.Equals (EventType.HardAcceleration) || e.EventType.Equals (EventType.HardBrake)
				orderby e.Time descending //pull the most recent even
				select e).Take(50).Skip(0); //skip the 1st "0" and grab 50 events

			var fetchedEvents = await ((MojioQueryable<Event>)fetchEventsQuery).FetchAsync ();
			foreach (var ev in fetchedEvents) {
				if (ev is TripEvent) {
					var tripEvent = ev as TripEvent;
					System.Diagnostics.Debug.WriteLine (tripEvent.EventType);
				}
			}
		}
	}
}

