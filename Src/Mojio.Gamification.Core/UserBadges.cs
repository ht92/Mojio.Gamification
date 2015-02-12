using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserBadges
	{
		[PrimaryKey]
		public string uid { get; set; }
		public string badgeCollectionData { get; set; }
	}
}