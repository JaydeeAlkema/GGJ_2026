using System.Collections.Generic;
using System.Linq;
using Mask;
using NaughtyAttributes;
using UnityEngine;

namespace Crafting
{
	public class CraftingComponentSpawner : MonoBehaviour
	{
		[BoxGroup("Settings")]
		[SerializeField] private Vector2 SpawnPosition = new(0, 0);

		private readonly List<MaskComponent> _spawnedComponents = new();

		private void OnEnable()
		{
			CraftingComponentMenuItem.OnComponentMenuItemClicked += SpawnComponent;
		}

		private void OnDisable()
		{
			CraftingComponentMenuItem.OnComponentMenuItemClicked -= SpawnComponent;

			Cleanup();
		}

		private void SpawnComponent(MaskComponent componentPrefab)
		{
			MaskComponent newComponent = Instantiate(componentPrefab, SpawnPosition, Quaternion.identity);
			newComponent.SetVisuals(newComponent.GetVisuals());
			_spawnedComponents.Add(newComponent);
		}

		private void Cleanup()
		{
			foreach (MaskComponent component in _spawnedComponents.Where(component => component != null))
			{
				Destroy(component.gameObject);
			}

			_spawnedComponents.Clear();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(SpawnPosition, 0.1f);
		}
	}
}
