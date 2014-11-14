using System;
using System.Collections.Generic;

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
}

