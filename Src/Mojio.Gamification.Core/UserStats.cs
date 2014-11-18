using System;
using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserStats
	{
		[PrimaryKey, AutoIncrement]
		public long uid { get; set; }

		public double totalDistance { get; set; }
		public int totalDuration { get; set; }
		public int safetyScore { get; set; }
		public int efficiencyScore { get; set; }
		public int numHardEvents { get; set; }
		public int totalIdleTime { get; set; }
		public double totalFuelConsumption { get; set; }
	}
}

