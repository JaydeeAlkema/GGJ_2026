using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mask
{
	public sealed class MaskInputController : MonoBehaviour
	{
		private static InputSystem_Actions _inputs;

		private readonly List<MaskComponent> _candidates = new(16);

		private Camera _camera;
		private MaskComponent _activeDrag;

		private void Awake()
		{
			_inputs = new InputSystem_Actions();
			_camera = Camera.main;
		}

		private void OnEnable()
		{
			_inputs.Player.Enable();

			_inputs.Player.Click.performed += OnClickStarted;
			_inputs.Player.Click.canceled += OnClickCanceled;
		}

		private void OnDisable()
		{
			_inputs.Player.Click.performed -= OnClickStarted;
			_inputs.Player.Click.canceled -= OnClickCanceled;

			_inputs.Player.Disable();
		}

		private void Update()
		{
			if (!_activeDrag)
				return;

			_activeDrag.FollowMouse();
		}

		private void OnClickStarted(InputAction.CallbackContext _)
		{
			if (_activeDrag != null)
				return;

			Vector2 mouseScreen = Mouse.current.position.ReadValue();
			Vector3 worldPos = ScreenToWorld(mouseScreen);

			_activeDrag = ResolveTopMost(worldPos);
			_activeDrag?.Drag(worldPos);
		}

		private void OnClickCanceled(InputAction.CallbackContext _)
		{
			if (!_activeDrag)
				return;

			_activeDrag.Drop();
			_activeDrag = null;
		}

		private MaskComponent ResolveTopMost(Vector3 worldPos)
		{
			_candidates.Clear();

			foreach (MaskComponent mask in FindObjectsByType<MaskComponent>(FindObjectsSortMode.None))
			{
				if (!mask.GetIsDraggable())
					continue;

				if (!IsMouseOverSprite(mask, worldPos))
					continue;

				_candidates.Add(mask);
			}

			if (_candidates.Count == 0)
				return null;

			_candidates.Sort(CompareByRenderOrder);
			return _candidates[0];
		}

		private static int CompareByRenderOrder(MaskComponent a, MaskComponent b)
		{
			SpriteRenderer ra = a.GetSpriteRenderer();
			SpriteRenderer rb = b.GetSpriteRenderer();

			if (ra.sortingLayerID == rb.sortingLayerID)
				return ra.sortingOrder != rb.sortingOrder ? rb.sortingOrder.CompareTo(ra.sortingOrder) : rb.transform.position.z.CompareTo(ra.transform.position.z);

			int la = SortingLayer.GetLayerValueFromID(ra.sortingLayerID);
			int lb = SortingLayer.GetLayerValueFromID(rb.sortingLayerID);

			return lb.CompareTo(la);
		}

		private Vector3 ScreenToWorld(Vector2 screenPos)
		{
			Vector3 pos = new(screenPos.x, screenPos.y, -_camera.transform.position.z);
			return _camera.ScreenToWorldPoint(pos);
		}

		private static bool IsMouseOverSprite(MaskComponent mask, Vector3 worldPos)
		{
			SpriteRenderer sr = mask.GetSpriteRenderer();
			Sprite sprite = sr.sprite;

			if (sprite == null)
				return false;

			Vector2 local = sr.transform.InverseTransformPoint(worldPos);

			float ppu = sprite.pixelsPerUnit;
			Vector2 pivot = sprite.pivot;
			Rect rect = sprite.rect;

			Vector2 pixelPos = new(
				pivot.x + local.x * ppu,
				pivot.y + local.y * ppu
			);

			if (pixelPos.x < 0 || pixelPos.y < 0 ||
			    pixelPos.x >= rect.width || pixelPos.y >= rect.height)
				return false;

			int x = Mathf.FloorToInt(rect.x + pixelPos.x);
			int y = Mathf.FloorToInt(rect.y + pixelPos.y);

			Color c = sprite.texture.GetPixel(x, y);
			return c.a > 0.1f;
		}
	}
}
