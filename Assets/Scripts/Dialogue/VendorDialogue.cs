using NaughtyAttributes;
using UnityEngine;

namespace Dialogue
{
	public class VendorDialogue : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private SpeechBubble SpeechBubble;
		[BoxGroup("References")]
		[SerializeField] private GameObject VisualRoot;
		[BoxGroup("References")]
		[SerializeField] private SpriteRenderer VendorSprite;

		[BoxGroup("Dialogue")]
		[SerializeField] private string[] Lines;

		public void Speak()
		{
			Show();

			if (SpeechBubble == null || Lines == null || Lines.Length == 0)
				return;

			string line = Lines[Random.Range(0, Lines.Length)];
			SpeechBubble.SetText(line);
			SpeechBubble.ShowText();
		}

		public void Show()
		{
			if (VisualRoot != null)
			{
				VisualRoot.SetActive(true);
			}
			else if (VendorSprite != null)
			{
				VendorSprite.enabled = true;
			}
		}

		public void Hide()
		{
			SpeechBubble?.HideText();
			if (VisualRoot != null)
			{
				VisualRoot.SetActive(false);
			}
			else if (VendorSprite != null)
			{
				VendorSprite.enabled = false;
			}
		}
	}
}
