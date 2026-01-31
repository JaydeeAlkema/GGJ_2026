using UnityEngine;

namespace Mask
{
	public interface IMaskComponent
	{
		Sprite GetVisuals();
		void SetVisuals(Sprite sprite);

		void SetClampArea(float clampTop, float clampBottom, float clampLeft, float clampRight);

		void SetMaskTraits(MaskTrait trait);
		MaskTrait GetMaskTraits();
		MaskComponentType GetMaskComponentType();

		bool GetIsLocked();
		void Lock();
		void Unlock();

		bool GetIsDraggable();
		void Drag(Vector3 worldPos);
		void Drop();

		void Scale(float value);

		void FlipHorizontal(bool flipLeft);
		void FlipVertical(bool flipUp);

		void FollowMouse();
	}
}
