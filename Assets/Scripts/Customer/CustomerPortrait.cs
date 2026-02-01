using Dialogue;
using Mask;
using NaughtyAttributes;
using Scoring;
using UnityEngine;
using UnityEngine.UI;

namespace Customer
{
	public class CustomerPortrait : MonoBehaviour
	{
		[BoxGroup("Portrait")]
		[SerializeField] private Image PortraitImage;

		[BoxGroup("Portrait")]
		[SerializeField] private SpeechBubble SpeechBubble;

		private Customer _customerData;
		private ScoreCalculator _scoreCalculator;

		private void OnEnable()
		{
			_scoreCalculator = new ScoreCalculator();
			SpeechBubble.HideText();
		}

		private void OnDisable()
		{
			_scoreCalculator = null;
		}

		public void Calculate(MaskTrait[] maskTraits)
		{
			_scoreCalculator.ResetScore();
			ScoreCalculator.ScoringResult result = _scoreCalculator.Calculate(_customerData, maskTraits);
			SpeechBubble.HideText();
			switch (result)
			{
				case ScoreCalculator.ScoringResult.Happy:
					SpeechBubble.SetEmotion(SpeechBubble.Emotion.Happy);
					break;
				case ScoreCalculator.ScoringResult.Neutral:
					SpeechBubble.SetEmotion(SpeechBubble.Emotion.Neutral);
					break;
				case ScoreCalculator.ScoringResult.Angry:
					SpeechBubble.SetEmotion(SpeechBubble.Emotion.Angry);
					break;
				default:
					SpeechBubble.SetEmotion(SpeechBubble.Emotion.Neutral);
					break;
			}

			SpeechBubble.ShowBubble();
		}

		public void SetCustomerData(Customer customer)
		{
			_customerData = customer;
			PortraitImage.sprite = _customerData.GetDialogueBoxSprite();
		}
	}
}
