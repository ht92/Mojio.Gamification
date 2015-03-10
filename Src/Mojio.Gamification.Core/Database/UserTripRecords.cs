using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserTripRecords
	{
		[PrimaryKey]
		public string uid { get; set; }
		public string tripHistoryData { get; set; }
	}
}

