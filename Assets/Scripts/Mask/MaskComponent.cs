using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mask
{
	public class MaskComponent : MonoBehaviour, IMaskComponent
	{
		public static event Action<MaskComponent> OnMaskComponentRemoveRequested;

		[BoxGroup("References")]
		[SerializeField] private SpriteRenderer SpriteRenderer;

		[BoxGroup("Mask")]
		[SerializeField] private MaskTrait Trait;
		[BoxGroup("Mask")]
		[SerializeField] private MaskComponentType ComponentType;
		[BoxGroup("Mask")]
		[SerializeField] private Sprite Visuals;
		[BoxGroup("Mask")]
		[SerializeField] private float VisualsSize;

		public SpriteRenderer GetSpriteRenderer()
		{
			return SpriteRenderer;
		}

		private bool _isLocked;
		private bool _isDragging;

		private float _clampTop;
		private float _clampBottom;
		private float _clampLeft;
		private float _clampRight;

		private float _scale = 1f;

		private InputSystem_Actions _inputs;
		private Camera _camera;
		private Collider2D _collider;

		private Vector3 _localGrabOffset;

		private void OnEnable()
		{
			_inputs = new InputSystem_Actions();
			_camera = Camera.main;
			_collider = GetComponent<Collider2D>();

			_inputs.Player.Enable();

			_inputs.Player.Scale.performed += OnScalePerformed;

			_inputs.Player.HorizontalFlip.performed += OnHorizontalFlipPerformed;
			_inputs.Player.VerticalFlip.performed += OnVerticalFlipPerformed;

			_inputs.Player.ChangeSpriteOrder.performed += OnChangeSpriteOrderPerformed;

			_scale = this.transform.localScale.x;
		}

		private void OnDisable()
		{
			_inputs.Player.Scale.performed -= OnScalePerformed;

			_inputs.Player.HorizontalFlip.performed -= OnHorizontalFlipPerformed;
			_inputs.Player.VerticalFlip.performed -= OnVerticalFlipPerformed;

			_inputs.Player.Disable();
		}

		private void OnScalePerformed(InputAction.CallbackContext obj)
		{
			if (!_isDragging)
				return;

			float scaleValue = obj.ReadValue<float>();
			Scale(scaleValue);
		}

		private void OnHorizontalFlipPerformed(InputAction.CallbackContext obj)
		{
			if (!_isDragging)
				return;

			float flipValue = obj.ReadValue<float>();
			bool flipLeft = flipValue < 0;
			FlipHorizontal(flipLeft);
		}

		private void OnVerticalFlipPerformed(InputAction.CallbackContext obj)
		{
			if (!_isDragging)
				return;

			float flipValue = obj.ReadValue<float>();
			bool flipUp = flipValue > 0;
			FlipVertical(flipUp);
		}

		private void OnChangeSpriteOrderPerformed(InputAction.CallbackContext obj)
		{
			if (!_isDragging)
				return;

			int orderChange = (int)obj.ReadValue<float>();
			orderChange = Mathf.Min(0, orderChange);
			SpriteRenderer.sortingOrder += orderChange;
		}

		public void SetVisuals(Sprite sprite)
		{
			Visuals = sprite;
			SpriteRenderer.gameObject.transform.localScale = new Vector3(VisualsSize, VisualsSize, VisualsSize);
			SpriteRenderer.sprite = Visuals;
		}

		public void SetClampArea(float clampTop, float clampBottom, float clampLeft, float clampRight)
		{
			_clampTop = clampTop;
			_clampBottom = clampBottom;
			_clampLeft = clampLeft;
			_clampRight = clampRight;
		}

		public void SetMaskTraits(MaskTrait trait)
		{
			Trait = trait;
		}

		public Sprite GetVisuals()
		{
			return Visuals;
		}

		public MaskTrait GetMaskTraits()
		{
			return Trait;
		}

		public MaskComponentType GetMaskComponentType()
		{
			return ComponentType;
		}

		public bool GetIsLocked()
		{
			return _isLocked;
		}

		public void Lock()
		{
			_isLocked = true;
		}

		public void Unlock()
		{
			_isLocked = false;
		}

		public bool GetIsDraggable()
		{
			return !_isLocked;
		}

		public void Drag(Vector3 mouseWorldPosition)
		{
			if (_isLocked || _isDragging)
				return;

			// Convert mouse position into local space
			Vector3 localMouse = this.transform.InverseTransformPoint(mouseWorldPosition);

			_localGrabOffset = localMouse;
			_isDragging = true;
		}

		public void Drop()
		{
			if (_isLocked)
				return;

			if (!_isDragging)
				return;

			_isDragging = false;
		}

		public void RequestRemove()
		{
			OnMaskComponentRemoveRequested?.Invoke(this);
		}

		public void Scale(float value)
		{
			// Clamp the scale value to a reasonable range
			_scale += value * 0.05f;
			_scale = Mathf.Clamp(_scale, 0.1f, 1.5f);
			this.transform.localScale = new Vector3(_scale, _scale, _scale);
		}

		public void FlipHorizontal(bool flipLeft)
		{
			SpriteRenderer.flipX = flipLeft;
		}

		public void FlipVertical(bool flipUp)
		{
			SpriteRenderer.flipY = flipUp;
		}

		public void FollowMouse()
		{
			if (!_isDragging)
				return;

			Vector3 mouseScreen = Mouse.current.position.ReadValue();
			mouseScreen.z = -Camera.main.transform.position.z;

			Vector3 mouseWorld = _camera.ScreenToWorldPoint(mouseScreen);

			// Transform the stored local offset back to world space
			Vector3 worldOffset = this.transform.TransformVector(_localGrabOffset);

			Vector3 target = mouseWorld - worldOffset;

			target.x = Mathf.Clamp(target.x, _clampLeft, _clampRight);
			target.y = Mathf.Clamp(target.y, _clampBottom, _clampTop);

			this.transform.position = target;
		}
	}
}
