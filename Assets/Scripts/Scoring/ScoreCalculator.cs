using System;
using Mask;

namespace Scoring
{
	public class ScoreCalculator
	{
		public enum ScoringResult
		{
			Happy,
			Neutral,
			Angry,
		}

		private int _totalScore;

		public void ResetScore()
		{
			_totalScore = 0;
		}

		public ScoringResult Calculate(Customer.Customer customer, MaskTrait[] traits, bool storeScore = false)
		{
			int score = 0;

			foreach (MaskTrait trait in traits)
			{
				if (Array.Exists(customer.GetPreferredTraits(), t => t == trait))
				{
					score += 10; // Preferred trait
				}
				else if (Array.Exists(customer.GetNeutralTraits(), t => t == trait))
				{
					score += 5; // Neutral trait
				}
				else if (Array.Exists(customer.GetDislikedTraits(), t => t == trait))
				{
					score -= 10; // Disliked trait
				}
			}

			if (storeScore)
			{
				_totalScore += score;
			}

			switch (score)
			{
				case >= 10:
					return ScoringResult.Happy;

				case >= 0:
					return ScoringResult.Neutral;

				default:
					return ScoringResult.Angry;
			}
		}
	}
}
