using System;
using System.Collections.Generic;
using System.Linq;
using Mask;
using NaughtyAttributes;
using UnityEngine;

namespace Crafting
{
	public class CraftingComponentMenu : MonoBehaviour
	{
		public static event Action OnMaskCompleted;
		public static event Action OnCraftingStageChanged;

		[BoxGroup("References")]
		[SerializeField] private CraftingComponentMenuItem CraftingComponentMenuItemPrefab;
		[BoxGroup("References")]
		[SerializeField] private Transform MenuContentParent;

		private int _craftingStageIndex;

		private List<MaskComponentDatabaseEntry> maskComponents;

		private void OnEnable()
		{
			// Not performant at all. But who cares. Not me ¯\_(ツ)_/¯ ~Jaydee
			MaskComponentsInventory.OnMaskComponentsChanged += MaskComponentsDatabase_OnMaskComponentsChanged;
			DepopulateMenu();
			PopulateMenu();
		}

		private void OnDisable()
		{
			MaskComponentsInventory.OnMaskComponentsChanged -= MaskComponentsDatabase_OnMaskComponentsChanged;
		}

		private void MaskComponentsDatabase_OnMaskComponentsChanged()
		{
			DepopulateMenu();
			PopulateMenu();
		}

		private void PopulateMenu()
		{
			maskComponents = new List<MaskComponentDatabaseEntry>(MaskComponentsInventory.Instance.GetMaskComponents());
			MaskComponentType craftingStageType = (MaskComponentType)_craftingStageIndex;
			for (int i = 0; i < maskComponents.Count; i++)
			{
				// Remove all components that are not of the same type as the stage.
				// Also remove all components that have an amount of zero.
				if (maskComponents[i].ComponentType == craftingStageType && (maskComponents[i].Amount > 0 || maskComponents[i].Amount == -1))
					continue;

				maskComponents.RemoveAt(i);
				i--;
			}

			foreach (MaskComponentDatabaseEntry maskComponent in maskComponents)
			{
				CraftingComponentMenuItem menuItem = Instantiate(CraftingComponentMenuItemPrefab, MenuContentParent);
				menuItem.Initialize(maskComponent.MaskComponent, maskComponent.Amount);
			}
		}

		private void DepopulateMenu()
		{
			foreach (Transform child in MenuContentParent.transform)
			{
				Destroy(child.gameObject);
			}
		}

		public void SubmitButton()
		{
			_craftingStageIndex++;
			OnCraftingStageChanged?.Invoke();
			DepopulateMenu();
			PopulateMenu();

			if (maskComponents.Any())
				return;

			OnMaskCompleted?.Invoke();
			_craftingStageIndex = 0;
		}
	}
}
