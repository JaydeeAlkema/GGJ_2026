using System.Collections.Generic;
using CompletedMask;
using Mask;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	public class InventoryMenuItem : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Image ItemImage;

		[BoxGroup("References")]
		[SerializeField] private Transform TraitIconsParent;

		[BoxGroup("References")]
		[SerializeField] private TraitIconsDatabaseSO TraitIconsDatabase;

		private Button button;
		private CompletedMaskItem itemData;

		private void OnEnable()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnButtonClick);
		}

		private void OnDisable()
		{
			button.onClick.RemoveListener(OnButtonClick);
		}

		private void OnButtonClick() { }

		public void SetupItem(CompletedMaskItem item)
		{
			ItemImage.sprite = item.GetSprite();
			CleanupTraitIconObjects();
			PopulateTraitIcons();
		}

		private void PopulateTraitIcons()
		{
			List<MaskTrait> traits = itemData.GetMaskTraits();
			foreach (MaskTrait maskTrait in traits)
			{
				GameObject newTraitIconGo = new($"TraitIcon - {maskTrait}");
				newTraitIconGo.transform.SetParent(TraitIconsParent);
				Image traitIconImage = newTraitIconGo.AddComponent<Image>();
				traitIconImage.sprite = TraitIconsDatabase.GetIconForTrait(maskTrait);
			}
		}

		private void CleanupTraitIconObjects()
		{
			foreach (Transform traitIcon in TraitIconsParent)
			{
				if (!traitIcon)
					continue;

				if (traitIcon == TraitIconsParent)
					continue;

				Destroy(traitIcon);
			}
		}
	}
}
