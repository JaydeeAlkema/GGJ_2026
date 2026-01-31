using Crafting;
using Mask;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory
{
	public class InventoryDriver : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private CompletedMaskItemsSO CompletedMaskItemsSO;

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
			CompletedMaskItemsSO.AddCompletedMaskItem(completedMaskItem);
		}
	}
}
