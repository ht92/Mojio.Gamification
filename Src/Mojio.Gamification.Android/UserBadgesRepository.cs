using System.Collections.Generic;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserBadgesRepository
	{
		private readonly DataManagerHelper _helper;
		private readonly ConnectionService _loginManager;
		private static UserBadgesRepository _instance;

		public static UserBadgesRepository GetInstance ()
		{
			if (_instance == null) {
				_instance = new UserBadgesRepository ();
			}
			return _instance;
		}

		private UserBadgesRepository ()
		{
			_helper = GamificationApp.GetInstance ().MyDataManagerHelper;
			_loginManager = GamificationApp.GetInstance ().MyConnectionService;
		}

		public void UpdateBadges (string badgeCollectionJson)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) {
				UserBadges userBadges = new UserBadges ();
				userBadges.uid = _loginManager.UserName;
				userBadges.badgeCollectionData = badgeCollectionJson;
				if (db.Update (userBadges) == 0) {
					db.Insert (userBadges);
				}
			}
		}

		public UserBadges GetUserBadges ()
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) {
				SQLiteCommand command = database.CreateCommand ("SELECT * FROM UserBadges WHERE uid = ?", _loginManager.UserName);
				List<UserBadges> userBadges = command.ExecuteQuery<UserBadges> ();
				return userBadges [0];
			}
		}
	}
}

