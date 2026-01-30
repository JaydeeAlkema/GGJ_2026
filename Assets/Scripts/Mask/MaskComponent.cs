using NaughtyAttributes;
using UnityEngine;

namespace Mask
{
	public class MaskComponent : MonoBehaviour, IMaskComponent
	{
		private static InputSystem_Actions _inputs;

		[BoxGroup("Mask")]
		[SerializeField] private MaskTraits MaskTraits;

		[BoxGroup("Mask")]
		[SerializeField] private Sprite Visuals;

		private bool _isLocked;

		private void Awake()
		{
			_inputs ??= new InputSystem_Actions();
		}

		public void SetVisuals(Sprite sprite)
		{
			Visuals = sprite;
		}

		public Sprite GetVisuals()
		{
			return Visuals;
		}

		public MaskTraits GetMaskTraits()
		{
			return MaskTraits;
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
