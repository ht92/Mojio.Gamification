using System;
using System.Collections.Generic;
using System.Linq;
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
			return trip.Distance.HasValue && trip.EndTime.HasValue;
		}

		public UserStats GetTripStats ()
		{
			UserStats tripStats = UserStats.CreateStats (this);
			return tripStats;
		}

		public static string Serialize (TripDataModel tripDataModel)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore };
			return JsonConvert.SerializeObject (tripDataModel, settings);
		}

		public static TripDataModel Deserialize (string json)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore };
			return JsonConvert.DeserializeObject<TripDataModel> (json, settings);
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
			FuelEfficiencyMetric = FuelEfficiencyMetric.CreateMetric (MyTrip.VehicleId, MyTrip.FuelEfficiency.Value, MyTrip.Distance.Value);
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
			double safetyScore = ScoreCalculator.CalculateWeightedScore (new List<KeyValuePair<double, int>>() { 
				new KeyValuePair<double, int> (hardEventScore, 1),
				new KeyValuePair<double, int> (accidentEventScore, AccidentEventMetric.Weight)
			});
			TripSafetyScore = safetyScore;
		}

		private void calculateTripEfficiencyScore()
		{
			double fuelEfficiencyScore = FuelEfficiencyScoreCalculator.CalculateScore (FuelEfficiencyMetric);
			double efficiencyScore = ScoreCalculator.CalculateWeightedScore (new List<KeyValuePair<double, int>> () { 
				new KeyValuePair<double, int> (fuelEfficiencyScore, 1)
			});
			TripEfficiencyScore = efficiencyScore;
		}
	}
}

