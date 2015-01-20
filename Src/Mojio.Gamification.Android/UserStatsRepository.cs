using System;
using Android.Content;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserStatsRepository
	{
		public event EventHandler UserStatsUpdatedEvent;

		private DataManagerHelper _helper;
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
			_helper = new DataManagerHelper (GamificationApp.GetInstance ());
		}

		public long AddUserStats (UserStats addUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.Insert (addUser);
				if (count > 0) {
					OnUserStatsUpdatedEvent (EventArgs.Empty);
				}
				return count;
			}
		}

		public long DeleteUserStats (UserStats deleteUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.Delete (deleteUser);	
				if (count > 0) {
					OnUserStatsUpdatedEvent (EventArgs.Empty);
				}
				return count;
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
				return database.Table<UserStats> ().ElementAt (0);
			}
		}
			
		protected void OnUserStatsUpdatedEvent (EventArgs e)
		{
			UserStatsUpdatedEvent (this, e);
		}
	}
}

