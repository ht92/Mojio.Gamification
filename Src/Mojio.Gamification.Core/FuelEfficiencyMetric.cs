using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public class FuelEfficiencyMetric : Metric
	{
		private const double MPG_TO_LITRE_PER_100KM = 235.214583;
		public double TotalConsumption { get; private set; }
		public double EstimateEfficiency { get; private set; }
		private TripEvent mIgnitionOffEvent;

		public FuelEfficiencyMetric (TripEvent tripEvent, double distance)
			: base (distance)
		{
			if (tripEvent.EventType != EventType.IgnitionOff) {
				throw new ArgumentException ("Illegal EventType: " + tripEvent.EventType.ToString ());
			}
			mIgnitionOffEvent = tripEvent;
			initMetric ();
		}

		private void initMetric() 
		{
			Measure = mIgnitionOffEvent.FuelEfficiency ?? 0;
			TotalConsumption = Measure * TripDistance / 100;
			EstimateEfficiency = getFuelEconomyEstimates ();
		}

		private double getFuelEconomyEstimates ()
		{
			Guid vehicleId = mIgnitionOffEvent.VehicleId;
			//send webservice call to http://www.fueleconomy.gov/feg/ws/index.shtml#ft2
			//get Combined MPG estimates
			double combinedMPGEstimate = 30; //test value
			return MPG_TO_LITRE_PER_100KM / combinedMPGEstimate;
		}
	}
}

