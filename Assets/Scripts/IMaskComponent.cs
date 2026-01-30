using UnityEngine;

public interface IMaskComponent
{
	void SetVisuals(Sprite sprite);
	Sprite GetVisuals();

	bool GetIsLocked();
	void Lock();
	void Unlock();

	void Drag();
	void Drop();
}
