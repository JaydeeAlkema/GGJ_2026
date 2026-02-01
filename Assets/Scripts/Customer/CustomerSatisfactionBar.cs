using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mask;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Customer
{
	public class CustomerSatisfactionBar : MonoBehaviour
	{
		public static event Action OnScoreBarVisualsUpdated;

		private enum ScoreLevels
		{
			Empty = 0,
			Low = 1,
			Medium = 2,
			High = 3,
		}

		[BoxGroup("Sprites")]
		[SerializeField] private Sprite ScoreBoardEmpty;
		[BoxGroup("Sprites")]
		[SerializeField] private Sprite ScoreBoardLow;
		[BoxGroup("Sprites")]
		[SerializeField] private Sprite ScoreBoardMedium;
		[BoxGroup("Sprites")]
		[SerializeField] private Sprite ScoreBoardHigh;

		[BoxGroup("References")]
		[SerializeField] private Image ScoreBarImage;

		private void OnEnable()
		{
			ScoreBarImage.enabled = false;
		}

		private void OnDisable()
		{
			ScoreBarImage.enabled = false;
		}

		public void SetScoreBarVisualsDependingOnTraits(List<MaskTrait> traits, Customer customer)
		{
			// We go through all the liked, neutral and disliked traits of the customer.
			// We then go through all the traits given. These are the traits of the mask and need to be compared to the customer's traits.
			// It goes as follows:
			//	Player gets the first reward if all the conditions below are true: They provide a mask with at least one accessory. The customer does not Hate any accessory on the mask.
			//	Player gets the second reward if all the conditions below are true:	They provide a mask with at least two accessories. They Like or Love at least one accessory on the mask. They don’t Dislike or Hate any accessory on the mask.
			//	Player gets the third reward if all the conditions: They provide a mask with at least three accessories. The mask only has accessories that the customer Likes or Loves (no dislikes, hates, or neutrals).

			ScoreBarImage.enabled = true;
			int accessoryCount = traits.Count;
			int likedCount = traits.Count(t => customer.GetPreferredTraits().Contains(t));
			int neutralCount = traits.Count(t => customer.GetNeutralTraits().Contains(t));
			int dislikedCount = traits.Count(t => customer.GetDislikedTraits().Contains(t));

			ScoreLevels scoreLevel = accessoryCount switch
			{
				>= 3 when dislikedCount == 0 && neutralCount == 0 => ScoreLevels.High,
				>= 2 when dislikedCount == 0 && likedCount >= 1 => ScoreLevels.Medium,
				>= 1 when dislikedCount == 0 => ScoreLevels.Low,
				_ => ScoreLevels.Empty,
			};

			ScoreBarImage.sprite = scoreLevel switch
			{
				ScoreLevels.Empty => ScoreBoardEmpty,
				ScoreLevels.Low => ScoreBoardLow,
				ScoreLevels.Medium => ScoreBoardMedium,
				ScoreLevels.High => ScoreBoardHigh,
			};

			StartCoroutine(SendScoreBarVisualsUpdatedEventAfterTime());
		}

		private static IEnumerator SendScoreBarVisualsUpdatedEventAfterTime()
		{
			Debug.Log("Sending ScoreBarVisualsUpdatedEventAfterTime");
			yield return new WaitForSeconds(3f);
			Debug.Log("Invoking OnScoreBarVisualsUpdated");
			OnScoreBarVisualsUpdated?.Invoke();
		}
	}
}
