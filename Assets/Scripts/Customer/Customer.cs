using Dialogue;
using NaughtyAttributes;
using UnityEngine;

namespace Customer
{
	public class Customer : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private SpeechBubble SpeechBubble;

		[BoxGroup("Dialogue")]
		[SerializeField] private string[] GreetingText;
		[BoxGroup("Dialogue")]
		[SerializeField] private string[] FarewellText;
		[BoxGroup("Dialogue")]
		[SerializeField] private string[] HappyResponseText;
		[BoxGroup("Dialogue")]
		[SerializeField] private string[] NeutralResponseText;
		[BoxGroup("Dialogue")]
		[SerializeField] private string[] AngryResponseText;
	}
}
