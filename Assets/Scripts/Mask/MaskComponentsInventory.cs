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
			// If it already exists, just increment the amount.
			if (MaskComponents.Exists(item => item.MaskComponent == component))
			{
				MaskComponents.Find(item => item.MaskComponent == component).Amount++;
				OnMaskComponentsChanged?.Invoke();
				return;
			}

			// Otherwise, add a new entry.
			MaskComponents.Add(new MaskComponentDatabaseEntry
			{
				MaskComponent = component,
				ComponentType = component.GetMaskComponentType(),
				Amount = 1,
			});
			OnMaskComponentsChanged?.Invoke();
		}

		public void RemoveMaskComponent(MaskComponent component)
		{
			// If it already exists, just decrement the amount.
			// We never fully remove them from the list to keep track of what has been used.
			// Just decrease to zero.
			Debug.Log("Attempting to remove one instance of " + component.name);
			if (!MaskComponents.Exists(item => item.MaskComponent.name == component.name))
				return;

			MaskComponents.Find(item => item.MaskComponent == component).Amount--;
			Debug.Log("Removed one instance of " + component.name + ". Remaining amount: " + MaskComponents.Find(item => item.MaskComponent == component).Amount);
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
