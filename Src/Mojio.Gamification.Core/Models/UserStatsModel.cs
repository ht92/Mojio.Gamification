using System;

namespace Mojio.Gamification.Core
{
	public class UserStatsModel
	{
		public int totalTrips { get; set; }
		public double totalDistance { get; set; }
		public double totalDuration { get; set; }
		public int totalHardAccelerations { get; set; }
		public int totalHardBrakes { get; set; }
		public int totalHardLefts { get; set; }
		public int totalHardRights { get; set; }
		public int totalAccidents { get; set; }
		public double totalFuelConsumption = double.NaN;
		public double safetyScore = double.NaN;
		public double efficiencyScore = double.NaN;

		public static UserStatsModel CreateStatsModel (TripDataModel tripData)
		{
			UserStatsModel stats = new UserStatsModel ();
			stats.totalTrips = 1;
			stats.totalDistance = tripData.MyTrip.Distance.Value;
			stats.totalDuration = (tripData.MyTrip.EndTime.Value - tripData.MyTrip.StartTime).TotalSeconds;
			stats.totalHardAccelerations = tripData.HardEventMetric.GetHardAccelerationCount ();
			stats.totalHardBrakes = tripData.HardEventMetric.GetHardBrakeCount ();
			stats.totalHardLefts = tripData.HardEventMetric.GetHardLeftCount ();
			stats.totalHardRights = tripData.HardEventMetric.GetHardRightCount ();
			stats.totalAccidents = tripData.AccidentEventMetric.Count;
			stats.totalFuelConsumption = tripData.FuelEfficiencyMetric.TotalConsumption;
			stats.safetyScore = tripData.TripSafetyScore;
			stats.efficiencyScore = tripData.TripEfficiencyScore;
			return stats;
		}

		public static UserStatsModel SumStatsModel (UserStatsModel baseStats, UserStatsModel additionStats)
		{
			UserStatsModel sum = new UserStatsModel ();
			sum.totalTrips = baseStats.totalTrips + additionStats.totalTrips;
			sum.totalDistance = baseStats.totalDistance + additionStats.totalDistance;
			sum.totalDuration = baseStats.totalDuration + additionStats.totalDuration;
			sum.totalHardAccelerations = baseStats.totalHardAccelerations + additionStats.totalHardAccelerations;
			sum.totalHardBrakes = baseStats.totalHardBrakes + additionStats.totalHardBrakes;
			sum.totalHardLefts = baseStats.totalHardLefts + additionStats.totalHardLefts;
			sum.totalHardRights = baseStats.totalHardRights + additionStats.totalHardRights;
			sum.totalAccidents = baseStats.totalAccidents + additionStats.totalAccidents;
			sum.totalFuelConsumption = addDoubleWithNanCheck (baseStats.totalFuelConsumption, additionStats.totalFuelConsumption);

			double weight = additionStats.totalDistance / sum.totalDistance;
			sum.safetyScore = Double.IsNaN (additionStats.safetyScore) ? baseStats.safetyScore : addDoubleWithNanCheck ((1 - weight) * baseStats.safetyScore, weight * additionStats.safetyScore);
			sum.efficiencyScore = Double.IsNaN (additionStats.efficiencyScore) ? baseStats.efficiencyScore : addDoubleWithNanCheck ((1 - weight) * baseStats.efficiencyScore, weight * additionStats.efficiencyScore);
			return sum;
		}

		private static double addDoubleWithNanCheck (double d1, double d2)
		{
			double result = d1 + d2;
			if (Double.IsNaN (result)) {
				result = !Double.IsNaN (d1) ? d1 : d2;
			}
			return result;
		}
	}
}

