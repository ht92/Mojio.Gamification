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
		public int overallScore { get; set; }
		public int numSpeeding { get; set; }
		public double distanceSpeeding { get; set; }
		public int numHarshEvents { get; set; }
		public int totalIdleTime { get; set; }
		public double totalFuelConsumption { get; set; }
	}
}

