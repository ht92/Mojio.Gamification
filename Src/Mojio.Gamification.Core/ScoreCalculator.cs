using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public class ScoreCalculator
	{
		public static ScoreWrapper CalculateOverallScore(IList<ScoreWrapper> scores)
		{
			int total = 0;
			foreach (ScoreWrapper score in scores) {
				total += score.Score;
			}
			int average = total / scores.Count;
			return ScoreWrapper.WrapScore (average);
		}
	}

	public class HardEventScoreCalculator 
	{
		public static ScoreWrapper CalculateScore(HardEventMetric metric)
		{
			double frequency = metric.Measure;
			int score;
			try {
				score = (int) (1 / frequency);
			} catch (DivideByZeroException e) {
				score = Metric.MAX_SCORE;
			}
			return ScoreWrapper.WrapScore (score);
		}
	}

	public class FuelEfficiencyScoreCalculator 
	{
		public static ScoreWrapper CalculateScore(FuelEfficiencyMetric metric)
		{
			double kmPerLitre = metric.Measure;
			int score = Metric.MAX_SCORE;
			double percentage = metric.Measure / metric.EstimateEfficiency;
			if (percentage < 1.0) {
				int pointDeduction = (int) (2 * (Metric.MAX_SCORE - percentage * Metric.MAX_SCORE));
				score = Math.Max (Metric.MAX_SCORE - pointDeduction, Metric.MIN_SCORE);
			}
			return ScoreWrapper.WrapScore (score);
		}
	}
}

