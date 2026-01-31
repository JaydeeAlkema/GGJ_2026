using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Dialogue
{
	public class SpeechBubble : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Transform ContentTransform;
		[BoxGroup("References")]
		[SerializeField] private TextMeshProUGUI DialogueText;

		private string _text;

		public void SetText(string text)
		{
			_text = text;
			DialogueText.text = _text;
		}

		public void Show()
		{
			DialogueText.text = _text;
			ContentTransform.gameObject.SetActive(true);
		}

		public void Hide()
		{
			ContentTransform.gameObject.SetActive(false);
		}
	}
}
