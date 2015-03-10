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
			List<double> scores = new List<double> { safetyScore, efficiencyScore };
			List<double> relevantScores = scores.Where (x => !Double.IsNaN (x)).ToList ();
			return relevantScores.Count != 0 ? relevantScores.Average () : double.NaN;
		}

		public static double CalculateWeightedScore(IList<KeyValuePair<double, double>> entries)
		{
			List<KeyValuePair<double, double>> usefulEntries = entries.Where (x => !Double.IsNaN (x.Key) && x.Value != 0).ToList ();
			double score = 0;
			if (usefulEntries.Count == 0) {
				score = Double.NaN;
			} else {
				double totalWeight = usefulEntries.Sum (x => x.Value);
				foreach (KeyValuePair<double, double> entry in usefulEntries) {
					if (Double.IsNaN (entry.Key) || entry.Value == 0)
						continue;
					score += entry.Key * entry.Value / totalWeight;
				}
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
			double score = Metric.MAX_SCORE;
			if (Double.IsNaN (metric.Measure)) {
				score = Double.NaN;
			} else {
				double percentage = metric.EstimateBaselineEfficiency / metric.Measure;
				if (percentage < 1.0) {
					double pointDeduction = 2 * (Metric.MAX_SCORE - percentage * Metric.MAX_SCORE);
					score = Math.Max (Metric.MAX_SCORE - pointDeduction, Metric.MIN_SCORE);
				}
			}
			return score;
		}
	}
}

