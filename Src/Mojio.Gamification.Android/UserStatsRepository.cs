using System;
using Android.Content;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserStatsRepository
	{
		private DataManagerHelper _helper;
		private static UserStatsRepository _instance;

		public static UserStatsRepository GetInstance(Context context)
		{
			if (_instance == null) {
				_instance = new UserStatsRepository(context);
			}
			return _instance;
		}

		private UserStatsRepository(Context context)
		{
			_helper = new DataManagerHelper (context);
		}

		public long AddUserStats (UserStats addUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				return db.Insert (addUser);	
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
				return db.Update (updateUser);	
			}
		}

		public UserStats GetUserStats()
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				return database.Table<UserStats> ().ElementAt (0);
			}
		}
	}
}

