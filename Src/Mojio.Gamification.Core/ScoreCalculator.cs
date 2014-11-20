using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public class ScoreCalculator
	{
		public static double CalculateOverallScore(IList<double> scores)
		{
			double total = 0;
			foreach (double score in scores) {
				total += score;
			}
			double average = total / scores.Count;
			return average;
		}
	}

	public class HardEventScoreCalculator 
	{
		public static double CalculateScore(HardEventMetric metric)
		{
			double frequency = metric.Measure;
			double score;
			try {
				score = 1 / frequency;
			} catch (DivideByZeroException e) {
				score = Metric.MAX_SCORE;
			}
			return score;
		}
	}

	public class FuelEfficiencyScoreCalculator 
	{
		public static double CalculateScore(FuelEfficiencyMetric metric)
		{
			double kmPerLitre = metric.Measure;
			double score = Metric.MAX_SCORE;
			double percentage = metric.Measure / metric.EstimateEfficiency;
			if (percentage < 1.0) {
				double pointDeduction = 2 * (Metric.MAX_SCORE - percentage * Metric.MAX_SCORE);
				score = Math.Max (Metric.MAX_SCORE - pointDeduction, Metric.MIN_SCORE);
			}
			return score;
		}
	}
}

