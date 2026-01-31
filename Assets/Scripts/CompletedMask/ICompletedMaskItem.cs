using System.Collections.Generic;
using Mask;
using UnityEngine;

namespace CompletedMask
{
	public interface ICompletedMaskItem
	{
		Sprite GetSprite();
		List<MaskTrait> GetMaskTraits();
	}
}
