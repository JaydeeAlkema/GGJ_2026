using UnityEngine;

namespace Mask
{
	public interface IMaskComponent
	{
		Sprite GetVisuals();
		void SetVisuals(Sprite sprite);

		MaskTraits GetMaskTraits();

		bool GetIsLocked();
		void Lock();
		void Unlock();

		void Drag();
		void Drop();
	}
}
