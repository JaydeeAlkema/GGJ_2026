using System.Collections.Generic;
using Mask;
using UnityEngine;

namespace CompletedMask
{
	public class CompletedMaskItem : ICompletedMaskItem
	{
		private readonly Sprite sprite;
		private readonly List<MaskTrait> maskTraits;

		public CompletedMaskItem(Sprite sprite, List<MaskTrait> maskTraits)
		{
			this.sprite = sprite;
			this.maskTraits = maskTraits;
		}

		public Sprite GetSprite()
		{
			return sprite;
		}

		public List<MaskTrait> GetMaskTraits()
		{
			return maskTraits;
		}
	}
}
