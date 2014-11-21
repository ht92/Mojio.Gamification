using System;

namespace Mojio.Gamification.Core
{
	public abstract class DistanceBasedMetric : Metric
	{
		protected double TripDistance;

		protected DistanceBasedMetric (double distance)
		{
			TripDistance = distance;
		}
	}
}

