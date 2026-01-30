using UnityEngine;

public class MaskComponent : MonoBehaviour, IMaskComponent
{
	private bool _isLocked;
	private Sprite _visuals;

	public void SetVisuals(Sprite sprite)
	{
		_visuals = sprite;
	}
	public Sprite GetVisuals()
	{
		return _visuals;
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
