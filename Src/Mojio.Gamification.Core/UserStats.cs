using System;
using SQLite;

namespace Mojio.Gamification.Core
{
	public class UserStats
	{
		[PrimaryKey, AutoIncrement]
		public long uid { get; set; }

		public int totalTrips { get; set; }
		public double totalDistance { get; set; }
		public double totalDuration { get; set; }
		public int totalHardAccelerations { get; set; }
		public int totalHardBrakes { get; set; }
		public int totalHardLefts { get; set; }
		public int totalHardRights { get; set; }
		public double totalIdleTime { get; set; }
		public double fuelEfficiency { get; set; }
		public double safetyScore { get; set; }
		public double efficiencyScore { get; set; }

		public static UserStats CreateStats (TripDataModel tripData)
		{
			UserStats stats = new UserStats ();
			stats.totalTrips = 1;
			stats.totalDistance = tripData.MyTrip.Distance.Value;
			stats.totalDuration = (tripData.MyTrip.EndTime.Value - tripData.MyTrip.StartTime).TotalSeconds;
			stats.totalHardAccelerations = tripData.HardAccelerationMetric.Count;
			stats.totalHardBrakes = tripData.HardBrakeMetric.Count;
			stats.totalHardLefts = tripData.HardLeftMetric.Count;
			stats.totalHardRights = tripData.HardRightMetric.Count;
			stats.totalIdleTime = tripData.MyTrip.IdleTime.Value;
			stats.fuelEfficiency = tripData.MyTrip.FuelEfficiency.Value;
			stats.safetyScore = tripData.TripSafetyScore;
			stats.efficiencyScore = tripData.TripEfficiencyScore;
			return stats;
		}

		public static UserStats SumStats (UserStats stats1, UserStats stats2)
		{
			UserStats sum = new UserStats ();
			sum.totalTrips = stats1.totalTrips + stats2.totalTrips;
			sum.totalDistance = stats1.totalDistance + stats2.totalDistance;
			sum.totalDuration = stats1.totalDuration + stats2.totalDuration;
			sum.totalHardAccelerations = stats1.totalHardAccelerations + stats2.totalHardAccelerations;
			sum.totalHardBrakes = stats1.totalHardBrakes + stats2.totalHardBrakes;
			sum.totalHardLefts = stats1.totalHardLefts + stats2.totalHardLefts;
			sum.totalHardRights = stats1.totalHardRights + stats2.totalHardRights;
			sum.totalIdleTime = stats1.totalIdleTime + stats2.totalIdleTime;
			sum.fuelEfficiency = stats1.fuelEfficiency + stats2.fuelEfficiency;

			double weight = stats2.totalDistance / sum.totalDistance;
			sum.safetyScore = (1 - weight)*stats1.safetyScore + weight*stats2.safetyScore;
			sum.efficiencyScore = (1 - weight) * stats1.efficiencyScore + weight * stats2.efficiencyScore;
			return sum;
		}
	}
}

