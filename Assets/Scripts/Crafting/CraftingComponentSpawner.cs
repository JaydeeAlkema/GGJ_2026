using System;
using System.Collections.Generic;
using System.Linq;
using CompletedMask;
using Customer;
using Mask;
using NaughtyAttributes;
using Snapshotter;
using StateMachine.States;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
	[ExecuteAlways]
	public class CraftingComponentSpawner : MonoBehaviour
	{
		public static event Action<MaskComponent> OnSpawnedComponentChanged;

		[BoxGroup("Reference Area (16:9)")]
		[SerializeField] private Vector2 ReferenceSize = new(16f, 9f);

		[BoxGroup("Reference Area (16:9)")]
		[SerializeField]
		private Rect NormalizedClampRect = new(
			0.1f,
			0.1f,
			0.8f,
			0.8f
		);

		[BoxGroup("Spawn Parent")]
		[SerializeField] private Transform SpawnParent;

		[BoxGroup("Runtime (Read Only)")]
		[SerializeField][ReadOnly]
		private Vector2 SpawnPosition;

		[BoxGroup("Runtime (Read Only)")]
		[ReadOnly]
		[SerializeField] private float ClampTop;
		[BoxGroup("Runtime (Read Only)")]
		[ReadOnly]
		[SerializeField] private float ClampBottom;
		[BoxGroup("Runtime (Read Only)")]
		[ReadOnly]
		[SerializeField] private float ClampLeft;
		[BoxGroup("Runtime (Read Only)")]
		[ReadOnly]
		[SerializeField] private float ClampRight;

		[BoxGroup("References")]
		[SerializeField] private CustomerQueueManager CustomerQueueManager;
		[BoxGroup("References")]
		[SerializeField] private CustomerPortrait CustomerPortrait;
		[BoxGroup("References")]
		[SerializeField] private CustomerSatisfactionBar CustomerSatisfactionBar;

		[Space]
		[BoxGroup("References")]
		[SerializeField] private Button SubmitButton;

		private readonly List<MaskComponent> _spawnedComponents = new();
		private MaskSnapshotter _maskSnapshotter;

		private void Awake()
		{
			_maskSnapshotter = FindObjectsByType<MaskSnapshotter>(FindObjectsSortMode.None).FirstOrDefault();
			UpdateClampArea();
		}

		private void OnEnable()
		{
			UpdateClampArea();

			CraftingComponentMenuItem.OnComponentMenuItemClicked += CraftingComponentMenuItem_OnComponentMenuItemClicked;
			CraftingComponentMenu.OnCraftingStageChanged += CraftingComponentMenu_OnCraftingStageChanged;
			MaskComponent.OnMaskComponentRemoveRequested += MaskComponent_OnMaskComponentRemoveRequested;

			StateMachine.StateMachine.OnStateChanged += StateMachine_OnStateChanged;
		}

		private void OnDisable()
		{
			CraftingComponentMenuItem.OnComponentMenuItemClicked -= CraftingComponentMenuItem_OnComponentMenuItemClicked;
			CraftingComponentMenu.OnCraftingStageChanged -= CraftingComponentMenu_OnCraftingStageChanged;
			MaskComponent.OnMaskComponentRemoveRequested -= MaskComponent_OnMaskComponentRemoveRequested;

			StateMachine.StateMachine.OnStateChanged -= StateMachine_OnStateChanged;

			Cleanup();
		}

		private void OnValidate()
		{
			UpdateClampArea();
		}

		private void Update()
		{
#if UNITY_EDITOR
			UpdateClampArea();
#endif
		}

		private void CraftingComponentMenuItem_OnComponentMenuItemClicked(MaskComponent componentPrefab)
		{
			MaskComponent newComponent = Instantiate(componentPrefab, SpawnPosition, Quaternion.identity, SpawnParent);
			newComponent.SetVisuals(newComponent.GetVisuals());
			newComponent.SetClampArea(ClampTop, ClampBottom, ClampLeft, ClampRight);
			_spawnedComponents.Add(newComponent);

			SubmitButton.interactable = true;
			if (newComponent.GetMaskComponentType() is MaskComponentType.Base)
				return;

			List<MaskTrait> maskTraits = new()
			{
				newComponent.GetMaskTraits(),
			};
			CustomerPortrait.Calculate(maskTraits.ToArray());

			MaskComponentsInventory.Instance.RemoveMaskComponent(componentPrefab);
			OnSpawnedComponentChanged?.Invoke(newComponent);
		}

		private void CraftingComponentMenu_OnCraftingStageChanged()
		{
			foreach (MaskComponent component in _spawnedComponents.Where(c => c != null))
			{
				component.Lock();
			}

			SubmitButton.interactable = false;
		}

		private void MaskComponent_OnMaskComponentRemoveRequested(MaskComponent component)
		{
			MaskComponentsInventory.Instance.AddMaskComponent(component);
			_spawnedComponents.Remove(component);
			Destroy(component.gameObject);
		}

		private void StateMachine_OnStateChanged(StateBase previousState, StateBase currentState)
		{
			Customer.Customer currentCustomer = CustomerQueueManager.GetCurrentCustomer();
			if (currentState is CraftingState)
			{
				CustomerPortrait.SetCustomerData(currentCustomer);
				SubmitButton.interactable = false;
			}
			else if (currentState is AfterCraftingState && previousState is CraftingState)
			{
				List<MaskTrait> maskTraits = new(
					_spawnedComponents
						.Where(x => x != null)
						.Select(x => x.GetMaskTraits())
						.ToList()
				);

				CustomerSatisfactionBar.SetScoreBarVisualsDependingOnTraits(maskTraits, currentCustomer);
			}
			else if (currentState is CustomerDialogueState && previousState is AfterCraftingState)
			{
				if (_maskSnapshotter == null)
					return;

				List<MaskTrait> maskTraits = new(
					_spawnedComponents
						.Where(x => x != null)
						.Select(x => x.GetMaskTraits())
						.ToList()
				);

				Sprite snapshotSprite = _maskSnapshotter.Snapshot();
				CompletedMaskItem completedMaskItem = new(snapshotSprite, maskTraits);
				CustomerQueueManager.SetCurrentCustomerMaskItem(completedMaskItem);

				Cleanup();
			}
		}

		private void UpdateClampArea()
		{
			Camera cam = Camera.main;
			if (cam == null || !cam.orthographic)
				return;

			float screenAspect = cam.aspect;
			float referenceAspect = ReferenceSize.x / ReferenceSize.y;

			float worldHeight = cam.orthographicSize * 2f;
			float worldWidth = worldHeight * screenAspect;

			float usableWidth = worldWidth;
			float usableHeight = worldHeight;

			if (screenAspect > referenceAspect)
			{
				usableWidth = worldHeight * referenceAspect;
			}
			else
			{
				usableHeight = worldWidth / referenceAspect;
			}

			float left = -usableWidth * 0.5f;
			float bottom = -usableHeight * 0.5f;

			ClampLeft = left + usableWidth * NormalizedClampRect.xMin;
			ClampRight = left + usableWidth * NormalizedClampRect.xMax;
			ClampBottom = bottom + usableHeight * NormalizedClampRect.yMin;
			ClampTop = bottom + usableHeight * NormalizedClampRect.yMax;

			SpawnPosition = new Vector2(
				(ClampLeft + ClampRight) * 0.5f,
				(ClampBottom + ClampTop) * 0.5f
			);
		}

		private void Cleanup()
		{
			foreach (MaskComponent component in _spawnedComponents.Where(c => c != null))
			{
				DestroyImmediate(component.gameObject);
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
