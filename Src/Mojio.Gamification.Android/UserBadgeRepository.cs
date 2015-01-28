using System;
using System.Collections.Generic;
using Android.Content;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserBadgeRepository
	{
		private DataManagerHelper _helper;
		private static UserBadgeRepository _instance;

		public static UserBadgeRepository GetInstance ()
		{
			if (_instance == null) {
				_instance = new UserBadgeRepository ();
			}
			return _instance;
		}

		private UserBadgeRepository ()
		{
			_helper = new DataManagerHelper (GamificationApp.GetInstance ());
		}

		public long UpdateBadge (UserBadge badge)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.Update (badge);	
				return count;
			}
		}

		public void UpdateBadges (List<UserBadge> badges)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path))
			{
				foreach (UserBadge badge in badges) {
					if (db.Update (badge) == 0) {
						db.Insert (badge);
					}
				}
			}
		}

		public List<UserBadge> GetUserBadges()
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				List<UserBadge> badges = new List<UserBadge> ();
				int count = database.Table<UserBadge> ().Count ();
				for (int index = 0; index < count; index++) {
					badges.Add (database.Table<UserBadge> ().ElementAt (index));
				}
				return badges;
			}
		}
	}
}

