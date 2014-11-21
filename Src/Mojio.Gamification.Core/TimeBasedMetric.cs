using System;

namespace Mojio.Gamification.Core
{
	public abstract class TimeBasedMetric : Metric
	{
		protected double TripDuration;

		protected TimeBasedMetric (double duration)
		{
			TripDuration = duration;
		}
	}
}

