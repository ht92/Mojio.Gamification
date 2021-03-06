﻿using System;

namespace Mojio.Gamification.Core
{
	public abstract class Metric
	{
		public const double MAX_SCORE = 100;
		public const double MIN_SCORE = 0;
		protected double Weight = 1;

		public double Measure { get; protected set; }

		public double GetWeight ()
		{
			return Weight;
		}

		public void SetWeight (double weight)
		{
			Weight = weight;
		}
	}
}

