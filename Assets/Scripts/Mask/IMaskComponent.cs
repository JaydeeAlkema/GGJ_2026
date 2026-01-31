using UnityEngine;

namespace Mask
{
	public interface IMaskComponent
	{
		Sprite GetVisuals();
		void SetVisuals(Sprite sprite);

		void SetClampArea(float clampTop, float clampBottom, float clampLeft, float clampRight);

		MaskTrait GetMaskTraits();
		MaskComponentType GetMaskComponentType();

		bool GetIsLocked();
		void Lock();
		void Unlock();

		void Drag();
		void Drop();
	}
}
