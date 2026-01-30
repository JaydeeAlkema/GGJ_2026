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

		[BoxGroup("Settings")]
		[SerializeField] private float ClampTop;
		[BoxGroup("Settings")]
		[SerializeField] private float ClampBottom;
		[BoxGroup("Settings")]
		[SerializeField] private float ClampLeft;
		[BoxGroup("Settings")]
		[SerializeField] private float ClampRight;

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
			newComponent.SetClampArea(ClampTop, ClampBottom, ClampLeft, ClampRight);
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

			Gizmos.color = Color.yellow;
			Vector3 topLeft = new(ClampLeft, ClampTop, 0);
			Vector3 topRight = new(ClampRight, ClampTop, 0);
			Vector3 bottomLeft = new(ClampLeft, ClampBottom, 0);
			Vector3 bottomRight = new(ClampRight, ClampBottom, 0);
			Gizmos.DrawLine(topLeft, topRight);
			Gizmos.DrawLine(topRight, bottomRight);
			Gizmos.DrawLine(bottomRight, bottomLeft);
			Gizmos.DrawLine(bottomLeft, topLeft);
		}
	}
}
