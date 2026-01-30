using System;

namespace Mask
{
	[Serializable]
	public struct MaskComponentItem : IEquatable<MaskComponentItem>
	{
		public MaskComponent MaskComponent;
		public MaskComponentType ComponentType;

		public MaskComponentItem(MaskComponent maskComponent, MaskComponentType componentType)
		{
			MaskComponent = maskComponent;
			ComponentType = componentType;
		}

		public bool Equals(MaskComponentItem other)
		{
			return Equals(MaskComponent, other.MaskComponent) && ComponentType == other.ComponentType;
		}

		public override bool Equals(object obj)
		{
			return obj is MaskComponentItem other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(MaskComponent, (int)ComponentType);
		}
	}
}
