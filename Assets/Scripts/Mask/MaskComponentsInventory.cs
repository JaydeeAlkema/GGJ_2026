using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

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
	}
}
