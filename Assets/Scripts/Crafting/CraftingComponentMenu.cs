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
		[SerializeField] private MaskComponentsSO MaskComponentsDatabase;
		[BoxGroup("References")]
		[SerializeField] private CraftingComponentMenuItem CraftingComponentMenuItemPrefab;
		[BoxGroup("References")]
		[SerializeField] private Transform MenuContentParent;

		private int _craftingStageIndex;

		private void OnEnable()
		{
			// Not performant at all. But who cares. Not me ¯\_(ツ)_/¯ ~Jaydee
			DepopulateMenu();
			PopulateMenu();
		}

		private void PopulateMenu()
		{
			List<IMaskComponent> maskComponentsInterfaces = MaskComponentsDatabase.GetMaskComponentsInterfaces();
			MaskComponentType craftingStageType = (MaskComponentType)_craftingStageIndex;
			for (int i = 0; i < maskComponentsInterfaces.Count; i++)
			{
				int maskComponentTypeIndex = (int)maskComponentsInterfaces[i].GetMaskComponentType();
				if (maskComponentTypeIndex == (int)craftingStageType)
					continue;

				maskComponentsInterfaces.RemoveAt(i);
				i--;
			}

			if (!maskComponentsInterfaces.Any())
			{
				OnMaskCompleted?.Invoke();
				return;
			}

			foreach (IMaskComponent maskComponent in maskComponentsInterfaces)
			{
				CraftingComponentMenuItem menuItem = Instantiate(CraftingComponentMenuItemPrefab, MenuContentParent);
				menuItem.Initialize((MaskComponent)maskComponent);
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
		}
	}
}
