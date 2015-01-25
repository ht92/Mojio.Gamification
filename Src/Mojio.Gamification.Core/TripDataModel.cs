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
		public HardEventMetric HardEventMetric;
		public FuelEfficiencyMetric FuelEfficiencyMetric;
		public double TripSafetyScore;
		public double TripEfficiencyScore;

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
			double safetyScore = ScoreCalculator.CalculateWeightedScore (new List<KeyValuePair<double, int>>() { 
				new KeyValuePair<double, int> (hardEventScore, 1),
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

