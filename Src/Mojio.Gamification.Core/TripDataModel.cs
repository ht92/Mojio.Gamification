using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public class TripDataModel
	{
		public Trip MyTrip { get; private set; }
		public HardEventMetric HardAccelerationMetric;
		public HardEventMetric HardBrakeMetric;
		public HardEventMetric HardLeftMetric;
		public HardEventMetric HardRightMetric;
		public FuelEfficiencyMetric FuelEfficiencyMetric;
		public double TripSafetyScore;
		public double TripEfficiencyScore;

		private IList<Event> mEvents;
	
		public TripDataModel (Trip trip, IList<Event> events)
		{
			MyTrip = trip;
			mEvents = events;
			initialize ();
		}

		public UserStats GetTripStats ()
		{
			UserStats tripStats = UserStats.CreateStats (this);
			return tripStats;
		}

		private void initialize() 
		{
			initializeMetrics ();
			calculateScores ();
		}

		private void initializeMetrics ()
		{
			HardAccelerationMetric = HardEventMetric.CreateMetric (EventType.HardAcceleration, mEvents, MyTrip.Distance.Value);
			HardBrakeMetric = HardEventMetric.CreateMetric (EventType.HardBrake, mEvents, MyTrip.Distance.Value);
			HardLeftMetric = HardEventMetric.CreateMetric (EventType.HardLeft, mEvents, MyTrip.Distance.Value);
			HardRightMetric = HardEventMetric.CreateMetric (EventType.HardRight, mEvents, MyTrip.Distance.Value);
			FuelEfficiencyMetric = FuelEfficiencyMetric.CreateMetric (MyTrip.VehicleId, MyTrip.FuelEfficiency.Value, MyTrip.Distance.Value);
		}

		private void calculateScores ()
		{
			calculateSafetyScore ();
			calculateEfficiencyScore ();
		}

		private void calculateSafetyScore()
		{
			double hardAccelerationScore = HardEventScoreCalculator.CalculateScore (HardAccelerationMetric);
			double hardBrakeScore = HardEventScoreCalculator.CalculateScore (HardBrakeMetric);
			double hardLeftScore = HardEventScoreCalculator.CalculateScore (HardLeftMetric);
			double hardRightScore = HardEventScoreCalculator.CalculateScore (HardRightMetric);
			double safetyScore = ScoreCalculator.CalculateOverallScore (new List<double> { 
				hardAccelerationScore,
				hardBrakeScore,
				hardLeftScore,
				hardRightScore
			});
			TripSafetyScore = safetyScore;
		}

		private void calculateEfficiencyScore()
		{
			double fuelEfficiencyScore = FuelEfficiencyScoreCalculator.CalculateScore (FuelEfficiencyMetric);
			double efficiencyScore = ScoreCalculator.CalculateOverallScore (new List<double> { fuelEfficiencyScore });
			TripEfficiencyScore = efficiencyScore;
		}
	}
}

