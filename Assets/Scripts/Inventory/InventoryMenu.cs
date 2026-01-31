using CompletedMask;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory
{
	public class InventoryMenu : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private CompletedMaskItemsSO CompletedMaskItems;
		[BoxGroup("References")]
		[SerializeField] private InventoryMenuItem MenuItemsPrefab;

		private void OnEnable()
		{
			PopulateMenu();
		}

		private void OnDisable()
		{
			DepopulateMenu();
		}

		private void PopulateMenu()
		{
			foreach (CompletedMaskItem item in CompletedMaskItems.GetCompletedMaskItems())
			{
				InventoryMenuItem newMenuItem = Instantiate(MenuItemsPrefab, this.transform);
				newMenuItem.SetupItem(item);
			}
		}

		private void DepopulateMenu()
		{
			foreach (Transform child in this.transform)
			{
				if (!child)
					continue;

				if (child == this.transform)
					continue;

				Destroy(child.gameObject);
			}
		}
	}
}
