using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserStats
	{
		[PrimaryKey]
		public string uid { get; set; }
		public string userStatsData { get; set; }

		public static UserStats CreateStats (string username)
		{
			UserStats stats = new UserStats ();
			UserStatsModel statsModel = new UserStatsModel ();
			stats.uid = username;
			stats.userStatsData = JsonSerializationUtility.Serialize (statsModel);
			return stats;
		}
	}
}

