using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

using Mojio;
using Mojio.Client;
using Mojio.Events;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class ConnectionService
	{
		public event EventHandler<LoginEventArgs> LoginEvent;

		private static ConnectionService _instance;

		public string UserName { get; private set; }
		private string Password;
		private MojioClient mClient;

		private readonly static LoginEventArgs LOGIN_SUCCESS = new LoginEventArgs (true);
		private readonly static LoginEventArgs LOGIN_FAIL = new LoginEventArgs (false);

		public static ConnectionService GetInstance ()
		{
			if (_instance == null)	{
				_instance = new ConnectionService ();
			} 
			return _instance;
		}

		private ConnectionService ()
		{
			mClient = new MojioClient();
		}

		public void Login (string username, string password)
		{
			UserName = username;
			Password = password;
			initializeConnection (username, password);
		}

		private async Task initializeConnection (string username, string password)
		{			
			//------------------Initializing the SDK----------------------//
			Guid appID = new Guid("32059d24-d66e-4071-b59e-0f97f1122c53");
			Guid secretKey = new Guid("929f3903-ede2-46c6-b532-eb39bcd06aa7");  //determines LIVE or SANDBOX
			var vehicleID = new Guid ("194e0cb4-9a93-4aed-a3a4-1e51b9cb33d5");

			var result = await mClient.BeginAsync (appID, secretKey); 	//return true if connected to the server  //takes a while to connect
			if (!result) {
				OnLoginFailEvent ();
			} else {
				//--------------Authenticating a Mojio User------------------//
				if (!mClient.IsLoggedIn ()) {
					var response = await mClient.SetUserAsync (username, password);
					if (response.StatusCode != HttpStatusCode.OK) {
						Logger.GetInstance ().Error (String.Format ("Failed to login: {0}.", response.ErrorMessage));
						OnLoginFailEvent ();
						return;
					}
				}
				Logger.GetInstance ().Info ("Connected to Mojio servers!");
				EventType[] types = { EventType.IgnitionOff, EventType.IgnitionOn, EventType.HardAcceleration };
				await mClient.Subscribe<Vehicle> (vehicleID, types);
				Logger.GetInstance ().Info ("Subscribed to Mojio events!");
				OnLoginSuccessfulEvent ();
			}
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

		private void OnLoginSuccessfulEvent ()
		{
			LoginEvent (this, LOGIN_SUCCESS);
		}

		private void OnLoginFailEvent ()
		{
			LoginEvent (this, LOGIN_FAIL);
		}

		public class LoginEventArgs : EventArgs 
		{
			public bool IsSuccess { get; set; }
			public LoginEventArgs (bool isSuccess) 
			{
				IsSuccess = isSuccess;
			}
		}
	}
}

