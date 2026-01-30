using Mask;
using NaughtyAttributes;
using UnityEngine;

namespace Crafting
{
	public class CraftingComponentMenu : MonoBehaviour
	{
		[BoxGroup("References")]
		[SerializeField] private MaskComponentsSO MaskComponentsDatabase;
		[BoxGroup("References")]
		[SerializeField] private CraftingComponentMenuItem CraftingComponentMenuItemPrefab;
		[BoxGroup("References")]
		[SerializeField] private Transform MenuContentParent;

		private void OnEnable()
		{
			// Not performant at all. But who cares. Not me ¯\_(ツ)_/¯ ~Jaydee
			DepopulateMenu();
			PopulateMenu();
		}

		private void PopulateMenu()
		{
			foreach (IMaskComponent maskComponent in MaskComponentsDatabase.GetMaskComponentsInterfaces())
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
	}
}
