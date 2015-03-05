using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Refractored.Xam.Settings;

using Mojio;
using Mojio.Client;
using Mojio.Events;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class ConnectionService
	{
		public event EventHandler<ConnectionEventArgs> LoginEvent;
		public event EventHandler<ConnectionEventArgs> LogoutEvent;

		private static ConnectionService _instance;

		public string UserName { get; private set; }
		private string Password;
		private MojioClient mClient;

		private readonly static ConnectionEventArgs SUCCESS = new ConnectionEventArgs (true);
		private readonly static ConnectionEventArgs FAIL = new ConnectionEventArgs (false);
		private readonly static EventType[] SUBSCRIBE_EVENTS = { EventType.IgnitionOff };

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

		public void Login ()
		{
			string username = CrossSettings.Current.GetValueOrDefault<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_username));
			string password = CrossSettings.Current.GetValueOrDefault<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_password));
			Login (username, password);
		}

		public void Login (string username, string password)
		{
			UserName = username;
			Password = password;
			initializeConnection (username, password);
		}

		public void Logout ()
		{
			uninitializeConnection ();
		}

		public bool IsConnected ()
		{
			return mClient.IsLoggedIn ();
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
					//var response = await mClient.SetUserAsync (username, password);
					var response = await mClient.SetUserAsync ("capstoneling", "GamificationofDriving2014");
					if (response.StatusCode != HttpStatusCode.OK) {
						Logger.GetInstance ().Error (String.Format ("Failed to login: {0}.", response.ErrorMessage));
						OnLoginFailEvent ();
						return;
					}
				}
				Logger.GetInstance ().Info ("Connected to Mojio servers!");
				mClient.EventHandler += (evt) => ReceiveEvent (evt);
				await mClient.Subscribe<Vehicle> (vehicleID, SUBSCRIBE_EVENTS);
				Logger.GetInstance ().Info ("Subscribed to Mojio events!");
				OnLoginSuccessfulEvent ();
			}
		}

		private async Task uninitializeConnection ()
		{
			var response = await mClient.ClearUserAsync ();
			if (response.StatusCode == HttpStatusCode.OK) {
				OnLogoutSuccessfulEvent ();
			} else {
				OnLogoutFailEvent ();
			}
		}
			
		private async Task ReceiveEvent (Event e)
		{
			if (e.EventType == EventType.IgnitionOff) {
				var latestTrip = await FetchLatestTripAsync ();
				Trip trip = latestTrip.Item1;
				List<Event> events = latestTrip.Item2;
				GamificationApp.GetInstance ().MyStatisticsManager.AddTrip (trip, events);
			}
		}
			
		public async Task<Tuple<Trip, List<Event>>> FetchLatestTripAsync () {

			//setting up mojioID/vehicleID 
			var response = await mClient.GetAsync<Mojio> ();
			var data = response.Data.Data.First(); //access the first simulator which is dummy simulator
			//------Fetching the trips from mojio server--------//

			// Fetch first page of 15 trips
			var results = await mClient.GetAsync<Trip> (1, null, true, null);
			//get the most recent trip because we only interested in the last trip.
			Trip mostRecentTrip = results.Data.Data.First();
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
			CrossSettings.Current.AddOrUpdateValue<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_username), UserName);
			CrossSettings.Current.AddOrUpdateValue<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_password), Password);
			LoginEvent (this, SUCCESS);
		}

		private void OnLoginFailEvent ()
		{
			LoginEvent (this, FAIL);
		}

		private void OnLogoutSuccessfulEvent ()
		{
			CrossSettings.Current.Remove (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_username));
			CrossSettings.Current.Remove (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_password));
			LogoutEvent (this, SUCCESS);
		}

		private void OnLogoutFailEvent ()
		{
			LogoutEvent (this, FAIL);
		}

		public class ConnectionEventArgs : EventArgs 
		{
			public bool IsSuccess { get; set; }
			public ConnectionEventArgs (bool isSuccess) 
			{
				IsSuccess = isSuccess;
			}
		}
	}
}

