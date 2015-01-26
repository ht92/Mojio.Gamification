using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public class ScoreCalculator
	{
		public static double CalculateOverallScore(double safetyScore, double efficiencyScore)
		{
			List<double> scores = new List<double> {
				safetyScore,
				efficiencyScore
			};
			return scores.Average ();
		}

		public static double CalculateWeightedScore(IList<KeyValuePair<double, int>> entries)
		{
			int totalWeight = entries.Sum(x => x.Value);
			double score = 0;
			foreach (KeyValuePair<double, int> entry in entries) {
				score += entry.Key * entry.Value / totalWeight;
			}
			return score;
		}
	}

	public class HardEventScoreCalculator 
	{
		public static double CalculateScore(HardEventMetric metric)
		{
			double frequency = metric.Measure;
			return Math.Min (1 / frequency, 100);
		}
	}

	public class AccidentEventScoreCalculator
	{
		public static double CalculateScore (AccidentEventMetric metric)
		{
			return metric.Count > 0 ? 0 : 100;
		}
	}

	public class FuelEfficiencyScoreCalculator 
	{
		public static double CalculateScore(FuelEfficiencyMetric metric)
		{
			double kmPerLitre = metric.Measure;
			double score = Metric.MAX_SCORE;
			double percentage = metric.EstimateEfficiency / metric.Measure;
			if (percentage < 1.0) {
				double pointDeduction = 2 * (Metric.MAX_SCORE - percentage * Metric.MAX_SCORE);
				score = Math.Max (Metric.MAX_SCORE - pointDeduction, Metric.MIN_SCORE);
			}
			return score;
		}
	}
}

