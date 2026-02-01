using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
	public class SpeechBubble : MonoBehaviour
	{
		public enum Emotion
		{
			Happy,
			Neutral,
			Angry,
		}

		[BoxGroup("References")]
		[SerializeField] private TextMeshProUGUI DialogueText;
		[BoxGroup("References")]
		[SerializeField] private Image DialogueTextBackground;

		[Space]
		[BoxGroup("References")]
		[SerializeField] private Image BubbleSprite;
		[BoxGroup("References")]
		[SerializeField] private Sprite HappySprite;
		[BoxGroup("References")]
		[SerializeField] private Sprite NeutralSprite;
		[BoxGroup("References")]
		[SerializeField] private Sprite AngrySprite;

		private string _text;

		public void SetEmotion(Emotion emotion)
		{
			HideText();
			BubbleSprite.sprite = emotion switch
			{
				Emotion.Happy => HappySprite,
				Emotion.Neutral => NeutralSprite,
				Emotion.Angry => AngrySprite,
				_ => BubbleSprite.sprite,
			};
		}

		public void SetText(string text)
		{
			_text = text;
			DialogueText.text = _text;
		}

		public void ShowText()
		{
			DialogueText.gameObject.SetActive(true);
			DialogueTextBackground.gameObject.SetActive(true);
		}

		public void HideText()
		{
			DialogueText.gameObject.SetActive(false);
			DialogueTextBackground.gameObject.SetActive(false);
		}

		public void ShowBubble()
		{
			BubbleSprite.gameObject.SetActive(true);
		}

		public void HideBubble()
		{
			BubbleSprite.gameObject.SetActive(false);
		}
	}
}
