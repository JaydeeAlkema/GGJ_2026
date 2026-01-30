using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Mask
{
	[CreateAssetMenu(fileName = "Mask Components", menuName = "ScriptableObjects/New Mask Components", order = 0)]
	public class MaskComponentsSO : ScriptableObject
	{
		[BoxGroup("Mask Components")]
		[SerializeField] private List<MaskComponent> MaskComponents = new();

		public void AddMaskComponent(MaskComponent component)
		{
			if (MaskComponents.Contains(component))
				return;

			MaskComponents.Add(component);
		}

		public void RemoveMaskComponent(MaskComponent component)
		{
			if (!MaskComponents.Contains(component))
				return;

			MaskComponents.Remove(component);
		}

		public List<MaskComponent> GetMaskComponents()
		{
			return MaskComponents;
		}

		public List<IMaskComponent> GetMaskComponentsInterfaces()
		{
			List<IMaskComponent> interfaces = MaskComponents.Cast<IMaskComponent>().ToList();
			return interfaces;
		}
	}
}
