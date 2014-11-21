using System;

namespace Mojio.Gamification.Core
{
	public class IdleTimeMetric : TimeBasedMetric
	{
		public double Percentage { get; private set; }

		public static IdleTimeMetric CreateMetric (double idleTime, double duration)
		{
			return new IdleTimeMetric (idleTime, duration);
		}

		private IdleTimeMetric (double idleTime, double duration)
			: base (duration)
		{
			Measure = idleTime;
			Percentage = idleTime / TripDuration;
		}
	}
}

