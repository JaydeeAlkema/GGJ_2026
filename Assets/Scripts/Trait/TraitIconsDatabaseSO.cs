using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Mask
{
	[CreateAssetMenu(fileName = "TraitIconsDatabaseSO", menuName = "ScriptableObjects/Trait Icons Database")]
	public class TraitIconsDatabaseSO : ScriptableObject
	{
		[BoxGroup("Trait Icons Database")]
		[SerializeField] private List<TraitIconEntry> TraitIconEntries = new();

		[BoxGroup("Trait Icons Database")]
		[SerializeField] private Sprite DefaultIcon;

		public Sprite GetIconForTrait(MaskTrait trait)
		{
			foreach (TraitIconEntry entry in TraitIconEntries.Where(entry => entry.Trait == trait))
			{
				return entry.Icon;
			}

			Debug.LogWarning($"No icon found for trait: {trait}");
			return DefaultIcon;
		}

		public MaskTrait GetTraitFromIcon(Sprite icon)
		{
			foreach (TraitIconEntry entry in TraitIconEntries.Where(entry => entry.Icon == icon))
			{
				return entry.Trait;
			}

			Debug.LogWarning($"No trait found for icon: {icon.name}");
			return default;
		}

		[Serializable]
		public struct TraitIconEntry
		{
			public MaskTrait Trait;
			public Sprite Icon;
		}
	}
}
