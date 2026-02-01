using NaughtyAttributes;
using StateMachine.States;
using UnityEngine;

namespace Dialogue
{
	public class VendorDialogue : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private SpriteRenderer VendorSprite;

		private void OnEnable()
		{
			StateMachine.StateMachine.OnStateChanged += StateMachineOnOnStateChanged;
		}

		private void OnDisable()
		{
			StateMachine.StateMachine.OnStateChanged -= StateMachineOnOnStateChanged;
		}

		private void StateMachineOnOnStateChanged(StateBase _, StateBase currentState)
		{
			switch (currentState)
			{
				case CraftingState:
				case AfterCraftingState:
					VendorSprite.enabled = false;
					break;

				case VendorDialogueState:
				case CustomerDialogueState:
					VendorSprite.enabled = true;
					break;
			}
		}
	}
}
