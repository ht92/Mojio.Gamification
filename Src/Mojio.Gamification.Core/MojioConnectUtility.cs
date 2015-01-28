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
	public class MojioConnectUtility
	{
		private static MojioConnectUtility _instance;
		private MojioClient mClient;

		public static MojioConnectUtility GetInstance ()
		{
			if (_instance == null) {
				_instance = new MojioConnectUtility ();
			}
			return _instance;
		}

		private MojioConnectUtility ()
		{
			mClient = new MojioClient(MojioClient.Live); 				//this is actually connected to the sandbox environment
			Task connectToMojio = initializeConnection ();
		}

		private async Task initializeConnection ()
		{			
			//------------------Initializing the SDK----------------------//
			Guid appID = new Guid("32059d24-d66e-4071-b59e-0f97f1122c53");
			Guid secretKey = new Guid("929f3903-ede2-46c6-b532-eb39bcd06aa7");
			var mojioId = new Guid ("194e0cb4-9a93-4aed-a3a4-1e51b9cb33d5");

			var result = await mClient.BeginAsync (appID, secretKey); 	//return true if connected to the server  //takes a while to connect
			//--------------Authenticating a Mojio User------------------//
			if (!mClient.IsLoggedIn ()) {
				var response = await mClient.SetUserAsync ("capstoneling", "GamificationofDriving2014");
				var token = response.Data;
				if (response.StatusCode != HttpStatusCode.OK) {
					throw new Exception (String.Format ("Failed to login: {0}.", response.ErrorMessage));
				}
			}
			await subscribeToEventsAsync ();
			Console.WriteLine ("Connected to Mojio servers!");
		}

		private async Task subscribeToEventsAsync ()
		{
			//--------------------Subscribing to SignalR Events--------------------------//
			// An array of event types you wish to be notified about
			var vehicleID = new Guid ("194e0cb4-9a93-4aed-a3a4-1e51b9cb33d5");
			EventType[] types = new EventType[] { EventType.IgnitionOff, EventType.IgnitionOn, EventType.HardAcceleration};
			mClient.EventHandler += ReceiveEvent;
			await mClient.Subscribe<Vehicle> (vehicleID, types);
			Console.WriteLine ("Subscribed to Mojio events!");
		}

		public async Task<Tuple<Trip, List<Event>>> FetchLatestTripAsync () {

			//setting up mojioID/vehicleID 
			var response = await mClient.GetAsync<Mojio> ();
			var data = response.Data.Data.First(); //access the first simulator which is dummy simulator
			//------Fetching the trips from mojio server--------//

			// Fetch first page of 15 trips
			mClient.PageSize = 15;
			var results = await mClient.GetAsync<Trip> ();
			//get the most recent trip because we only interested in the last trip.
			Trip mostRecentTrip = results.Data.Data.Last();		
			Guid mostRecentTripId = mostRecentTrip.Id;

			// Fetch all events by Trip ID
			MojioResponse<Results<Event>> tripData = await mClient.GetByAsync<Event, Trip> (mostRecentTripId);
			if (tripData.StatusCode != HttpStatusCode.OK) {
				throw new Exception (String.Format ("Error retrieving latest trip {0}: {1}.", mostRecentTripId, tripData.StatusCode));
			}

			List<Event> tripEvents = tripData.Data.Data.ToList ();
			return Tuple.Create (mostRecentTrip, tripEvents);
		}


		//----------------function use for subscribing to SignalR Events----------------//
		// Setup the callback function

		static int IgnitionOff = 0;
		static int IgnitionOn = 0;

		public static void ReceiveEvent(Event ABC)
		{
			if( ABC.EventType == EventType.IgnitionOff) {
				// The car is now stopped. Call the function to start pulling data from the last trip
				// ...
				IgnitionOff += 1;  //used to test whether an ignitionOff event is received
			}
			else if( ABC.EventType == EventType.IgnitionOn) {
				// The car has just started
				// ...
				IgnitionOn += 1; 
			}
		}
	}
}

