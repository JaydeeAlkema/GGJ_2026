using Inventory;
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
			InventoryDriver.OnItemAddedToInventory += InventoryDriver_OnItemAddedToInventory;
		}

		private void OnDisable()
		{
			InventoryDriver.OnItemAddedToInventory -= InventoryDriver_OnItemAddedToInventory;
		}

		private void InventoryDriver_OnItemAddedToInventory()
		{
			CraftingMenuScreen.gameObject.SetActive(false);
			ClientMenuScreen.gameObject.SetActive(true);
		}
	}
}
