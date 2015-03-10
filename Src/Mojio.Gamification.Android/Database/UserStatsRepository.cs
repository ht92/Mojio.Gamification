using System;
using System.Collections.Generic;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserStatsRepository
	{
		public event EventHandler UserStatsUpdatedEvent;

		private readonly DataManagerHelper _helper;
		private readonly ConnectionService mConnectionService;
		private static UserStatsRepository _instance;

		public static UserStatsRepository GetInstance()
		{
			if (_instance == null) {
				_instance = new UserStatsRepository();
			}
			return _instance;
		}

		private UserStatsRepository ()
		{
			_helper = GamificationApp.GetInstance ().MyDataManagerHelper;
			mConnectionService = GamificationApp.GetInstance ().MyConnectionService;
		}

		public long AddUserStats (UserStats addUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				return  db.Insert (addUser);
			}
		}

		public long DeleteUserStats (UserStats deleteUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				return db.Delete (deleteUser);
			}
		}

		public long UpdateUserStats (UserStats updateUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.Update (updateUser);	
				if (count > 0) {
					OnUserStatsUpdatedEvent (EventArgs.Empty);
				}
				return count;
			}
		}

		public UserStats GetUserStats()
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				SQLiteCommand command = database.CreateCommand ("SELECT * FROM UserStats WHERE uid = ?", mConnectionService.CurrentUserName);
				List<UserStats> userStatsList = command.ExecuteQuery<UserStats> ();
				return userStatsList[0];
			}
		}

		public bool DoesUserExist (string userId)
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				SQLiteCommand command = database.CreateCommand ("SELECT * FROM UserStats WHERE uid = ?", userId);
				List<UserStats> userStatsList = command.ExecuteQuery<UserStats> ();
				return userStatsList.Count > 0;
			}
		}
			
		protected void OnUserStatsUpdatedEvent (EventArgs e)
		{
			UserStatsUpdatedEvent (this, e);
		}
	}
}

