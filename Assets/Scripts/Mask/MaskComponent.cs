using System;
using NaughtyAttributes;
using Unity.VisualScripting;
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
		[SerializeField] private Sprite Visuals;

		private bool _isLocked;

		private void Awake()
		{
			_inputs ??= new InputSystem_Actions();
		}

		private void OnEnable()
		{
			_inputs.Player.Enable();
			_inputs.Player.Click.performed += OnButtonClick;
			_inputs.Player.Click.canceled += OnButtonRelease;
		}
		
		private void OnButtonClick(InputAction.CallbackContext _)
		{
			
		}
		
		private void OnButtonRelease(InputAction.CallbackContext _)
		{
			
		}

		public void SetVisuals(Sprite sprite)
		{
			Visuals = sprite;
			SpriteRenderer.sprite = Visuals;
		}

		public Sprite GetVisuals()
		{
			return Visuals;
		}

		public MaskTrait GetMaskTraits()
		{
			return Trait;
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
			if (!_isLocked)
			{
				// Implement drag logic here
			}
		}

		public void Drop()
		{
			if (!_isLocked)
			{
				// Implement drop logic here
			}
		}
	}
}
