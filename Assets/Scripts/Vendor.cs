using CompletedMask;
using Dialogue;
using Mask;
using NaughtyAttributes;
using Scoring;
using UnityEngine;
using Random = UnityEngine.Random;
using StateMachine.States;

namespace Vendor
{
	public class Vendor : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private SpeechBubble SpeechBubble;
		[BoxGroup("References")]
		[SerializeField] private Transform MaskDisplayPoint;

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

				

		[BoxGroup("UI")]
		[SerializeField] private Sprite DialogueBoxSprite;

		private SpriteRenderer _spriteRenderer;
		private CompletedMaskItem _currentMaskItem;
		private ScoreCalculator _scoreCalculator;

		private void OnEnable()
		{
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			_scoreCalculator = new ScoreCalculator();
			StateMachine.StateMachine.OnStateChanged += StateMachine_OnStateChanged;

		}

		private void OnDisable()
		{
			_spriteRenderer = null;
			_scoreCalculator = null;
		}


		public Sprite GetDialogueBoxSprite()
		{
			return DialogueBoxSprite;
		}

		public string GetRandomHappyResponseText()
		{
			return HappyResponseText[Random.Range(0, HappyResponseText.Length)];
		}

		public string GetRandomNeutralResponseText()
		{
			return NeutralResponseText[Random.Range(0, NeutralResponseText.Length)];
		}

		public string GetRandomAngryResponseText()
		{
			return AngryResponseText[Random.Range(0, AngryResponseText.Length)];
		}

		public void SetCurrentMaskItem(CompletedMaskItem maskItem)
		{
			_currentMaskItem = maskItem;

			// Create a mask GameObject on the mask display point
			GameObject maskObject = new("CustomerMask");
			SpriteRenderer maskSpriteRenderer = maskObject.AddComponent<SpriteRenderer>();
			maskSpriteRenderer.sprite = maskItem.GetSprite();
			maskSpriteRenderer.sortingOrder = 10; // Ensure it's rendered above the customer
			maskObject.transform.SetParent(MaskDisplayPoint, false);
			maskObject.transform.localScale = Vector3.one * 0.25f; // Scale down the mask to fit the display point
		}

		

		public void Hide()
		{
			_spriteRenderer.enabled = false;
			SpeechBubble.Hide();
		}

		public void Show()
		{
			_spriteRenderer.enabled = true;
			SpeechBubble.Show();
		}

		public void HideSpeechBubble()
		{
			SpeechBubble.Hide();
		}

		public void ShowSpeechBubble()
		{
			SpeechBubble.Show();
		}

		public void Greet()
		{
			string greeting = GreetingText[Random.Range(0, GreetingText.Length)];
			SpeechBubble.SetText(greeting);
			SpeechBubble.Show();
		}

		public void Farewell()
		{
			string farewell = FarewellText[Random.Range(0, FarewellText.Length)];
			SpeechBubble.SetText(farewell);
			SpeechBubble.Show();
		}

		private void StateMachine_OnStateChanged(StateBase previousState, StateBase currentState)
		{
			switch (currentState)
			{
				case VendorDialogueState:
					break;
			}
		}
	}
}

