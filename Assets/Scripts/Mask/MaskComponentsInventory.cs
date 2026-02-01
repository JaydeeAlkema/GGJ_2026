using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = System.Random;

namespace Mask
{
	public class MaskComponentsInventory : MonoBehaviour
	{
		public static MaskComponentsInventory Instance { get; private set; }

		public static event Action OnMaskComponentsChanged;

		[BoxGroup("Mask Components")]
		[SerializeField] private List<MaskComponentDatabaseEntry> MaskComponents = new();

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
				return;
			}

			Instance = this;
		}

		public void AddMaskComponent(MaskComponent component)
		{
			string nameToFind = component.name;
			string nameToFindSanitized = nameToFind.Replace("(Clone)", "").Trim();
			// If it already exists, just increment the amount.
			MaskComponentDatabaseEntry foundComponent = MaskComponents.Find(item => item.MaskComponent.name == nameToFindSanitized);
			if (foundComponent != null)
			{
				if (foundComponent.Amount < 0)
				{
					OnMaskComponentsChanged?.Invoke();
					return;
				}

				foundComponent.Amount++;
				OnMaskComponentsChanged?.Invoke();
				return;
			}

			// Otherwise, add a new entry.
			MaskComponentDatabaseEntry maskComponentDatabaseEntry = new()
			{
				MaskComponent = component,
				ComponentType = component.GetMaskComponentType(),
				Amount = 1,
			};
			MaskComponents.Add(maskComponentDatabaseEntry);
			Debug.Log($"Added new item with amount: {maskComponentDatabaseEntry.Amount} for component {maskComponentDatabaseEntry.MaskComponent.name}");
			OnMaskComponentsChanged?.Invoke();
		}

		public void RemoveMaskComponent(MaskComponent component)
		{
			// If it already exists, just decrement the amount.
			// We never fully remove them from the list to keep track of what has been used.
			// Just decrease to zero.
			string nameToFind = component.name;
			string nameToFindSanitized = nameToFind.Replace("(Clone)", "").Trim();
			MaskComponentDatabaseEntry foundComponent = MaskComponents.Find(item => item.MaskComponent.name == nameToFindSanitized);
			if (foundComponent == null)
				return;

			if (foundComponent.Amount < 0)
				return;

			foundComponent.Amount--;
			OnMaskComponentsChanged?.Invoke();
		}

		public List<MaskComponentDatabaseEntry> GetMaskComponents()
		{
			return MaskComponents;
		}

#if UNITY_EDITOR
		[Button]
		private void AddOneAmountToAllComponents()
		{
			foreach (MaskComponentDatabaseEntry entry in MaskComponents)
			{
				entry.Amount++;
			}
		}

		[Button]
		private void RemoveOneAmountFromAllComponents()
		{
			foreach (MaskComponentDatabaseEntry entry in MaskComponents)
			{
				entry.Amount = Mathf.Max(0, entry.Amount - 1);
			}
		}

		[Button]
		private void AddOneAmountToAllBaseComponents()
		{
			foreach (MaskComponentDatabaseEntry entry in MaskComponents.Where(entry => entry.ComponentType is MaskComponentType.Base))
			{
				entry.Amount++;
			}
		}

		[Button]
		private void RemoveOneAmountFromAllBaseComponents()
		{
			foreach (MaskComponentDatabaseEntry entry in MaskComponents.Where(entry => entry.ComponentType is MaskComponentType.Base))
			{
				entry.Amount = Mathf.Max(0, entry.Amount - 1);
			}
		}

		[Button]
		private void AddOneAmountToAllAccessoryComponents()
		{
			foreach (MaskComponentDatabaseEntry entry in MaskComponents.Where(entry => entry.ComponentType is MaskComponentType.Accessory))
			{
				entry.Amount++;
			}
		}

		[Button]
		private void RemoveOneAmountFromAllAccessoryComponents()
		{
			foreach (MaskComponentDatabaseEntry entry in MaskComponents.Where(entry => entry.ComponentType is MaskComponentType.Accessory))
			{
				entry.Amount = Mathf.Max(0, entry.Amount - 1);
			}
		}

		[Button]
		private void RandomizeAllAccessoryComponentTypes()
		{
			Array traitValues = Enum.GetValues(typeof(MaskTrait));
			Random random = new();

			foreach (MaskComponentDatabaseEntry entry in MaskComponents.Where(entry => entry.ComponentType is MaskComponentType.Accessory))
			{
				MaskTrait randomTrait = (MaskTrait)traitValues.GetValue(random.Next(traitValues.Length));
				entry.MaskComponent.SetMaskTraits(randomTrait);
			}
		}

		[Button]
		private void RemoveNullReferences()
		{
			for (int i = MaskComponents.Count - 1; i >= 0; i--)
			{
				if (MaskComponents[i].MaskComponent != null)
					continue;

				MaskComponents.RemoveAt(i);
			}
		}
#endif
	}
}
