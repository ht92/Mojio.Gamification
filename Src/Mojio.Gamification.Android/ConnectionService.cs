using System;
using System.Collections.Generic;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class ConnectionService
	{
		public event EventHandler LoginSuccessfulEvent;

		private static ConnectionService _instance;

		public MojioConnectUtility MyMojioConnectUtility { get; private set; }
		public string UserName { get; private set; }
		private string Password;

		public static ConnectionService GetInstance ()
		{
			if (_instance == null)	{
				_instance = new ConnectionService ();
			} 
			return _instance;
		}

		private ConnectionService ()
		{
			MyMojioConnectUtility = MojioConnectUtility.GetInstance ();
		}

		public bool Login (string username, string password)
		{
			UserName = username;
			Password = password;

			if (!connectToMojio (username, password)) {
				return false;
			}

			if (!GamificationApp.GetInstance ().MyUserStatsRepository.DoesUserExist (username)) {
				//create entries for new user in our tables
				GamificationApp.GetInstance ().MyUserStatsRepository.AddUserStats (UserStats.CreateStats (username));
				GamificationApp.GetInstance ().MyUserBadgesRepository.UpdateBadges (JsonSerializationUtility.Serialize (AchievementManager.DEFAULT_BADGE_COLLECTION));
				GamificationApp.GetInstance ().MyUserTripRecordsRepository.UpdateTripRecords (JsonSerializationUtility.Serialize (new List<TripDataModel> ()));
			}
			OnLoginSuccessfulEvent (EventArgs.Empty);
			return true;
		}

		private bool connectToMojio (string username, string password)
		{
			try {
				MyMojioConnectUtility.InitializeConnection (username, password);
				return true;
			} catch (Exception e) {
				Logger.GetInstance ().Error (String.Format("Failed to connect to Mojio servers - {0}", e.Message));
				return false;
			}
		}

		private void OnLoginSuccessfulEvent (EventArgs e)
		{
			LoginSuccessfulEvent (this, e);
		}
	}
}

