using System;

namespace Mojio.Gamification.Core
{
	public abstract class DistanceBasedMetric : Metric
	{
		protected double TripDistance;

		protected DistanceBasedMetric ()
		{
			TripDistance = 0;
		}

		protected DistanceBasedMetric (double distance)
		{
			TripDistance = distance;
		}
	}
}

