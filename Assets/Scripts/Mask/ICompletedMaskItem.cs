using System.Collections.Generic;
using UnityEngine;

namespace Mask
{
	public interface ICompletedMaskItem
	{
		Sprite GetSprite();
		List<MaskTrait> GetMaskTraits();
	}
}
