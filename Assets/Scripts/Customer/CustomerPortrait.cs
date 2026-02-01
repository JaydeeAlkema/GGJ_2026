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
			SpeechBubble.Hide();
		}

		private void OnDisable()
		{
			_scoreCalculator = null;
		}

		public void Calculate(MaskTrait[] maskTraits)
		{
			_scoreCalculator.ResetScore();
			ScoreCalculator.ScoringResult result = _scoreCalculator.Calculate(_customerData, maskTraits);
			SpeechBubble.Show();
			switch (result)
			{
				case ScoreCalculator.ScoringResult.Happy:
					SpeechBubble.SetText(_customerData.GetRandomHappyResponseText());
					break;
				case ScoreCalculator.ScoringResult.Neutral:
					SpeechBubble.SetText(_customerData.GetRandomNeutralResponseText());
					break;
				case ScoreCalculator.ScoringResult.Angry:
					SpeechBubble.SetText(_customerData.GetRandomAngryResponseText());
					break;
				default:
					SpeechBubble.SetText(_customerData.GetRandomNeutralResponseText());
					break;
			}
		}

		public void SetCustomerData(Customer customer)
		{
			_customerData = customer;
			PortraitImage.sprite = _customerData.GetDialogueBoxSprite();
		}
	}
}
