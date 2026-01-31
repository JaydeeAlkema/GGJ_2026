using CompletedMask;
using Crafting;
using NaughtyAttributes;
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
			CraftingComponentSpawner.OnCompletedMaskItemReady += CraftingComponentSpawner_OnCompletedMaskItemReady;
		}

		private void OnDisable()
		{
			CraftingComponentSpawner.OnCompletedMaskItemReady -= CraftingComponentSpawner_OnCompletedMaskItemReady;
		}

		private void CraftingComponentSpawner_OnCompletedMaskItemReady(CompletedMaskItem completedMaskItem)
		{
			ToggleClientScreen();
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
