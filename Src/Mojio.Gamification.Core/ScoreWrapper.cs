using System;

namespace Mojio.Gamification.Core
{
	public class ScoreWrapper
	{
		public enum ScoreRank { S, A, B, C, D, F };

		public int Score { get; private set;}
		public ScoreRank Rank { get; private set;}

		public static ScoreWrapper WrapScore(int score)
		{
			return new ScoreWrapper (score);
		}

		private ScoreWrapper (int score)
		{
			SetScore (score);
		}

		public void SetScore(int score)
		{
			if (score < 0) { score = 0; } 
			else if (score > 100) {	score = 100; }

			Score = score;
			if (score < 50) { Rank = ScoreRank.F; } 
			else if (score < 60) { Rank = ScoreRank.D; } 
			else if (score < 70) { Rank = ScoreRank.C; } 
			else if (score < 80) { Rank = ScoreRank.B; }
			else if (score < 90) { Rank = ScoreRank.A; } 
			else { Rank = ScoreRank.S; }
		}
	}
}

