using Mask;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
	public class CraftingComponentMenuItem : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private Image IconImage;

		private MaskComponent _maskComponentPrefab;

		public void Initialize(MaskComponent prefab)
		{
			_maskComponentPrefab = prefab;
			IconImage.sprite = prefab.GetVisuals();
		}

		public void OnSelect()
		{
			// Handle selection logic, e.g., adding to crafting area
		}
	}
}
