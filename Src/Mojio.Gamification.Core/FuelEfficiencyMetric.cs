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

		public static FuelEfficiencyMetric CreateMetric (Guid vehicleId, double fuelEfficiency, double distance)
		{
			return new FuelEfficiencyMetric (vehicleId, fuelEfficiency, distance);
		}

		private FuelEfficiencyMetric (Guid vehicleId, double fuelEfficiency, double distance)
			: base (distance)
		{
			Measure = fuelEfficiency;
			TotalConsumption = Measure * TripDistance / 100;
			EstimateEfficiency = getFuelEconomyEstimates (vehicleId);
		}

		private double getFuelEconomyEstimates (Guid vehicleId)
		{
			//send webservice call to http://www.fueleconomy.gov/feg/ws/index.shtml#ft2
			//get Combined MPG estimates
			double combinedMPGEstimate = 30; //test value
			return MPG_TO_LITRE_PER_100KM / combinedMPGEstimate;
		}
	}
}

