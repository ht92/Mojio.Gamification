using System.Collections.Generic;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserTripRecordsRepository
	{
		private readonly DataManagerHelper _helper;
		private readonly ConnectionService _loginManager;
		private static UserTripRecordsRepository _instance;

		public static UserTripRecordsRepository GetInstance ()
		{
			if (_instance == null) {
				_instance = new UserTripRecordsRepository ();
			}
			return _instance;
		}

		private UserTripRecordsRepository ()
		{
			_helper = GamificationApp.GetInstance ().MyDataManagerHelper;
			_loginManager = GamificationApp.GetInstance ().MyConnectionService;
		}
		

		public void UpdateTripRecords (string tripRecordsJson)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) {
				UserTripRecords tripRecords = new UserTripRecords ();
				tripRecords.uid = _loginManager.UserName;
				tripRecords.tripHistoryData = tripRecordsJson;
				if (db.Update (tripRecords) == 0) {
					db.Insert (tripRecords);
				}
			}
		}

		public UserTripRecords GetTripRecords ()
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) {
				SQLiteCommand command = database.CreateCommand ("SELECT * FROM UserTripRecords WHERE uid = ?", _loginManager.UserName);
				List<UserTripRecords> userTripRecords = command.ExecuteQuery<UserTripRecords> ();
				return userTripRecords [0];		
			}
		}
	}
}

