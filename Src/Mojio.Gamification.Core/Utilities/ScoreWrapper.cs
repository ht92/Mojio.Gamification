using System;

namespace Mojio.Gamification.Core
{
	public class ScoreWrapper
	{
		public enum ScoreRank { 
			S, 
			A, 
			B, 
			C, 
			D, 
			F, 
			NA 
		}

		public double Score { get; private set;}
		public ScoreRank Rank { get; private set;}

		public static ScoreWrapper WrapScore(double score)
		{
			return new ScoreWrapper (score);
		}

		private ScoreWrapper (double score)
		{
			SetScore (score);
		}

		private void SetScore(double score)
		{
			if (score < 0) { score = 0; } 
			else if (score > 100) {	score = 100; }

			Score = score;
			if (score < 50) { Rank = ScoreRank.F; } 
			else if (score < 60) { Rank = ScoreRank.D; }
			else if (score < 70) { Rank = ScoreRank.C; }
			else if (score < 80) { Rank = ScoreRank.B; }
			else if (score < 90) { Rank = ScoreRank.A; }
			else if (score > 90) { Rank = ScoreRank.S; }
			else { Rank = ScoreRank.NA; }
		}
	}
}

