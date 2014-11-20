using System;

namespace Mojio.Gamification.Core
{
	public abstract class Metric
	{
		public const double MAX_SCORE = 100;
		public const double MIN_SCORE = 0;
		public double Measure { get; protected set; }
		protected double TripDistance;
		public Metric (double distance)
		{
			TripDistance = distance;
		}
	}
}

