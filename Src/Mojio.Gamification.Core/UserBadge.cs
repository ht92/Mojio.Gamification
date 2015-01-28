using System;
using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserBadge
	{
		[PrimaryKey]
		public string badgeName { get; set; }
		public string badgeData { get; set; }
	}
}

