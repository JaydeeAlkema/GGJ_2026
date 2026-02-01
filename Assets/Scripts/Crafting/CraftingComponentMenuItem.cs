using System;
using Mask;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
	public class CraftingComponentMenuItem : MonoBehaviour
	{
		public static event Action<MaskComponent> OnComponentMenuItemClicked;

		[BoxGroup("References")]
		[SerializeField] private Image ComponentImage;

		[BoxGroup("References")]
		[SerializeField] private GameObject TraitImageContainer;
		[BoxGroup("References")]
		[SerializeField] private Image TraitImage;

		[BoxGroup("References")]
		[SerializeField] private GameObject AmountTextContainer;
		[BoxGroup("References")]
		[SerializeField] private TextMeshProUGUI AmountText;

		[Space]
		[BoxGroup("References")]
		[SerializeField] private TraitIconsDatabaseSO TraitIconsDatabase;

		private Button _button;
		private MaskComponent _maskComponentPrefab;

		private void Awake()
		{
			_button = GetComponentInChildren<Button>();
		}

		private void OnEnable()
		{
			_button.onClick.AddListener(OnButtonClick);
		}

		private void OnDisable()
		{
			_button.onClick.RemoveListener(OnButtonClick);
		}

		private void OnButtonClick()
		{
			OnComponentMenuItemClicked?.Invoke(_maskComponentPrefab);
		}

		public MaskTrait GetMaskTrait()
		{
			return _maskComponentPrefab.GetMaskTraits();
		}

		public void Initialize(MaskComponent prefab, int amount)
		{
			_maskComponentPrefab = prefab;
			ComponentImage.sprite = prefab.GetVisuals();

			bool componentIsBase = prefab.GetMaskComponentType() is MaskComponentType.Base;
			TraitImageContainer.SetActive(!componentIsBase);
			TraitImage.enabled = !componentIsBase;
			AmountTextContainer.SetActive(!componentIsBase);
			AmountText.enabled = !componentIsBase;

			TraitImage.sprite = TraitIconsDatabase.GetIconForTrait(prefab.GetMaskTraits());
			AmountText.text = amount.ToString();
		}
	}
}
