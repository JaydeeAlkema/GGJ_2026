using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mask
{
	public class MaskComponent : MonoBehaviour, IMaskComponent
	{
		private static InputSystem_Actions _inputs;

		[BoxGroup("References")]
		[SerializeField] private SpriteRenderer SpriteRenderer;

		[BoxGroup("Mask")]
		[SerializeField] private MaskTrait Trait;
		[BoxGroup("Mask")]
		[SerializeField] private MaskComponentType ComponentType;
		[BoxGroup("Mask")]
		[SerializeField] private Sprite Visuals;

		private bool _isLocked;
		private bool _isDragging;

		private float _clampTop;
		private float _clampBottom;
		private float _clampLeft;
		private float _clampRight;

		private static Camera _camera;
		private Collider2D _collider;

		private void Awake()
		{
			_inputs ??= new InputSystem_Actions();
			_camera ??= Camera.main;
			_collider = GetComponent<Collider2D>();
		}

		private void OnEnable()
		{
			_inputs.Player.Enable();
			_inputs.Player.Click.performed += OnButtonClick;
			_inputs.Player.Click.canceled += OnButtonRelease;
		}

		private void Update()
		{
			if (!_isDragging)
				return;

			FollowMouse();
		}

		private void OnDisable()
		{
			_inputs.Player.Click.performed -= OnButtonClick;
			_inputs.Player.Click.canceled -= OnButtonRelease;
			_inputs.Player.Disable();
		}

		private void OnButtonClick(InputAction.CallbackContext _)
		{
			Drag();
		}

		private void OnButtonRelease(InputAction.CallbackContext _)
		{
			Drop();
		}

		public void SetVisuals(Sprite sprite)
		{
			Visuals = sprite;
			SpriteRenderer.sprite = Visuals;
		}

		public void SetClampArea(float clampTop, float clampBottom, float clampLeft, float clampRight)
		{
			_clampTop = clampTop;
			_clampBottom = clampBottom;
			_clampLeft = clampLeft;
			_clampRight = clampRight;
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

		public void Drag()
		{
			if (_isLocked)
				return;

			if (_isDragging)
				return;

			// check if the mouse is over this component
			Vector3 mousePosition = Mouse.current.position.ReadValue();
			Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
			if (_collider == null || !_collider.OverlapPoint(worldPosition))
				return;

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

		private void FollowMouse()
		{
			Vector3 mousePosition = Mouse.current.position.ReadValue();
			mousePosition.z = 10f; // Set this to be the distance from the camera
			// clamp the position within the clamp area
			Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
			worldPosition.x = Mathf.Clamp(worldPosition.x, _clampLeft, _clampRight);
			worldPosition.y = Mathf.Clamp(worldPosition.y, _clampBottom, _clampTop);
			this.transform.position = worldPosition;
		}
	}
}
