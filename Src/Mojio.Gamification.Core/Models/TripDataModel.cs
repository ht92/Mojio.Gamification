using System.Collections.Generic;
using Mojio.Events;
using Newtonsoft.Json;

namespace Mojio.Gamification.Core
{
	public class TripDataModel
	{
		public Trip MyTrip { get; set; }

		//Safety Metrics
		public HardEventMetric HardEventMetric;
		public AccidentEventMetric AccidentEventMetric;

		//Efficiency Metrics
		public FuelEfficiencyMetric FuelEfficiencyMetric;

		//Calculated Scores
		public double TripSafetyScore;
		public double TripEfficiencyScore;

		//Events associated with trip
		private IList<Event> mEvents;

		[JsonConstructor]
		public TripDataModel ()
		{
		}
	
		public TripDataModel (Trip trip, IList<Event> events)
		{
			MyTrip = trip;
			mEvents = events;
			initialize ();
		}

		public static bool IsTripValid(Trip trip)
		{
			return trip.Distance.HasValue && trip.Distance.Value > 0 && trip.EndTime.HasValue;
		}

		public UserStatsModel GetTripStats ()
		{
			UserStatsModel tripStats = UserStatsModel.CreateStatsModel (this);
			return tripStats;
		}

		private void initialize() 
		{
			initializeMetrics ();
			calculateTripScores ();
		}

		private void initializeMetrics ()
		{
			HardEventMetric = HardEventMetric.CreateMetric (mEvents, MyTrip.Distance.Value);
			AccidentEventMetric = AccidentEventMetric.CreateMetric (mEvents, MyTrip.Distance.Value);
			FuelEfficiencyMetric = FuelEfficiencyMetric.CreateMetric (MyTrip.VehicleId, MyTrip.FuelEfficiency ?? double.NaN, MyTrip.Distance.Value);
		}

		private void calculateTripScores ()
		{
			calculateTripSafetyScore ();
			calculateTripEfficiencyScore ();
		}

		private void calculateTripSafetyScore()
		{
			double hardEventScore = HardEventScoreCalculator.CalculateScore (HardEventMetric);
			double accidentEventScore = AccidentEventScoreCalculator.CalculateScore (AccidentEventMetric);
			double safetyScore = ScoreCalculator.CalculateWeightedScore (new List<KeyValuePair<double, double>> { 
				new KeyValuePair<double, double> (hardEventScore, HardEventMetric.GetWeight ()),
				new KeyValuePair<double, double> (accidentEventScore, AccidentEventMetric.GetWeight ())
			});
			TripSafetyScore = safetyScore;
		}

		private void calculateTripEfficiencyScore()
		{
			double fuelEfficiencyScore = FuelEfficiencyScoreCalculator.CalculateScore (FuelEfficiencyMetric);
			double efficiencyScore = ScoreCalculator.CalculateWeightedScore (new List<KeyValuePair<double, double>> { 
				new KeyValuePair<double, double> (fuelEfficiencyScore, FuelEfficiencyMetric.GetWeight ())
			});
			TripEfficiencyScore = efficiencyScore;
		}
	}
}

