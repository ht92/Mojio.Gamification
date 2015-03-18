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

		private string mUsername;
		private string mPassword;
		private MojioClient mClient;
		public User CurrentUser { get; private set; }
		public string CurrentUserName { get; private set; }
		public List<Mojio> CurrentUserDevices { get; private set; }

		private readonly static ConnectionEventArgs SUCCESS = new ConnectionEventArgs (true);
		private readonly static ConnectionEventArgs FAIL = new ConnectionEventArgs (false);
		private readonly static EventType[] SUBSCRIBE_EVENTS = { EventType.IgnitionOff };

		private readonly static Guid APP_ID = new Guid("32059d24-d66e-4071-b59e-0f97f1122c53");
		private readonly static Guid SANDBOX_KEY = new Guid("929f3903-ede2-46c6-b532-eb39bcd06aa7");  //SANDBOX
		private readonly static Guid LIVE_KEY = new Guid("072d6485-300b-40f8-9507-ef0197125db6");  //LIVE
		private readonly static Guid SIMULATOR_ID = new Guid ("194e0cb4-9a93-4aed-a3a4-1e51b9cb33d5");

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
			mClient.PageSize = 30;
		}

		public void Login ()
		{
			string username = CrossSettings.Current.GetValueOrDefault<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_username));
			string password = CrossSettings.Current.GetValueOrDefault<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_password));
			Login (username, password);
		}

		public void Login (string username, string password)
		{
			mUsername = username;
			mPassword = password;
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
			var result = await mClient.BeginAsync (APP_ID, LIVE_KEY); 	//return true if connected to the server  //takes a while to connect
			if (!result) {
				OnLoginFailEvent ();
			} else {
				//--------------Authenticating a Mojio User------------------//
				if (!mClient.IsLoggedIn ()) {
					var setUserResponse = await mClient.SetUserAsync ("alaraozturk@hotmail.com", "Capstone2015");
					if (setUserResponse.StatusCode != HttpStatusCode.OK) {
						Logger.GetInstance ().Error (String.Format ("Failed to login: {0}.", setUserResponse.ErrorMessage));
						OnLoginFailEvent ();
						return;
					}
				}
				CurrentUser = await mClient.GetCurrentUserAsync ();
				CurrentUserName = CurrentUser.UserName ?? CurrentUser.Email;
				CurrentUserDevices = (await mClient.UserMojiosAsync (CurrentUser.Id)).Data.Data.ToList ();
				Logger.GetInstance ().Info ("Connected to Mojio servers!");
				await subscribeToEvents ();
				Logger.GetInstance ().Info ("Subscribed to Mojio events!");
				OnLoginSuccessfulEvent ();
			}
		}

		private async Task subscribeToEvents ()
		{
			mClient.EventHandler += receiveEvent;
			var response = await mClient.GetAsync<Mojio> ();
			List<Mojio> mojioDevices = response.Data.Data.ToList ();
			foreach (Mojio device in mojioDevices) {
				var vehicleId = device.VehicleId.HasValue ? device.VehicleId.Value : SIMULATOR_ID;
				await mClient.Subscribe<Vehicle> (vehicleId, SUBSCRIBE_EVENTS);
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
			
		private void receiveEvent (Event e)
		{
			if (e.EventType == EventType.IgnitionOff) {
				FetchLatestTripsSinceLastReceivedAsync ();
			}
		}
			
		public async Task FetchLatestTripsSinceLastReceivedAsync ()
		{
			List<Tuple<Trip, List<Event>>> mostRecentTrips = new List<Tuple<Trip, List<Event>>> ();
			TripDataModel lastReceivedTrip = GamificationApp.GetInstance ().MyTripHistoryManager.GetLatestRecord ();
			string tripStartTime = lastReceivedTrip != null ? String.Format ("StartTime={0}", lastReceivedTrip.MyTrip.StartTime.AddSeconds(1).ToString ("yyyy.MM.dd HH:mm:ss-")) : null;
			var results = await mClient.GetAsync<Trip> (sortBy: t => t.StartTime, desc: true, criteria: tripStartTime); //sort the trip based on the last to first trip
			List<Trip> trips = (List<Trip>)results.Data.Data;
			foreach (Trip trip in trips) {
				List<Event> relevantTripEvents = await fetchRelevantTripEventsAsync (trip);
				GamificationApp.GetInstance ().MyStatisticsManager.AddTrip (trip, relevantTripEvents);
			}
		}

		private async Task<List<Event>> fetchRelevantTripEventsAsync (Trip trip)
		{
			List<Event> relevantTripEvents = new List<Event> ();
			int index = 1;
			while (true) {
				List<Event> tripEvents = await fetchRelevantTripEventsByPageAsync (trip, index);
				if (tripEvents.Count == 0) break;
				relevantTripEvents.AddRange (tripEvents);
				index++;
			}
			return relevantTripEvents;
		}

		private async Task<List<Event>> fetchRelevantTripEventsByPageAsync (Trip trip, int page)
		{
			MojioResponse<Results<Event>> response = await mClient.GetByAsync<Event, Trip> (trip.Id, page);
			if (response.StatusCode != HttpStatusCode.OK) {
				throw new Exception (String.Format ("Error retrieving trip events {0}.", trip.Id));
			}
			return response.Data.Data.ToList ();
		}

		private void OnLoginSuccessfulEvent ()
		{
			CrossSettings.Current.AddOrUpdateValue<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_username), mUsername);
			CrossSettings.Current.AddOrUpdateValue<string> (GamificationApp.GetInstance ().Resources.GetString (Resource.String.settings_password), mPassword);
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

