using NaughtyAttributes;
using StateMachine.States;
using UnityEngine;

namespace UI
{
	// I hate classes like this. But it's a game jam, so who cares. ~Jaydee
	public class UIManager : MonoBehaviour
	{
		[BoxGroup("Menu Screens")]
		[SerializeField] private RectTransform CraftingMenuScreen;
		[BoxGroup("Menu Screens")]
		[SerializeField] private RectTransform ClientMenuScreen;

		private void OnEnable()
		{
			StateMachine.StateMachine.OnStateChanged += StateMachine_OnStateChanged;
		}

		private void OnDisable()
		{
			StateMachine.StateMachine.OnStateChanged -= StateMachine_OnStateChanged;
		}

		private void StateMachine_OnStateChanged(StateBase previousState, StateBase newState)
		{
			switch (newState)
			{
				case CraftingState:
					ToggleCraftingScreen();
					break;

				case VendorDialogueState:
				case CustomerDialogueState:
					ToggleClientScreen();
					break;
			}
		}

		private void ToggleClientScreen()
		{
			CraftingMenuScreen.gameObject.SetActive(false);
			ClientMenuScreen.gameObject.SetActive(true);
		}

		private void ToggleCraftingScreen()
		{
			ClientMenuScreen.gameObject.SetActive(false);
			CraftingMenuScreen.gameObject.SetActive(true);
		}
	}
}
