using System;
using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserBadge
	{
		[PrimaryKey, AutoIncrement]
		public int badgeId { get; set; }
		public string badgeName { get; set; }
		public int badgeType { get; set; }
		public int badgeLevel { get; set; }
		public int badgeMaxLevel { get; set; }
	}
}

