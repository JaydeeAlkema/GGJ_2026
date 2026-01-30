using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mask
{
	[CreateAssetMenu(fileName = "Mask Components", menuName = "ScriptableObjects/Mask Components", order = 0)]
	public class MaskComponentsSO : ScriptableObject
	{
		[BoxGroup("Mask Components")]
		[SerializeField] private List<MaskComponentItem> MaskComponents = new();

		public void AddMaskComponent(MaskComponentItem component)
		{
			if (MaskComponents.Contains(component))
				return;

			MaskComponents.Add(component);
		}

		public void RemoveMaskComponent(MaskComponentItem component)
		{
			if (!MaskComponents.Contains(component))
				return;

			MaskComponents.Remove(component);
		}

		public List<MaskComponentItem> GetMaskComponents()
		{
			return MaskComponents;
		}

		public List<IMaskComponent> GetMaskComponentsInterfaces()
		{
			return Enumerable.ToList(Enumerable.Cast<IMaskComponent>(Enumerable.Select(MaskComponents, item => item.MaskComponent)));
		}


#if UNITY_EDITOR
		private readonly List<MaskComponentItem> maskComponentsCopy = new();

		private void OnValidate()
		{
			maskComponentsCopy.Clear();
			maskComponentsCopy.AddRange(MaskComponents);

			// subscribe to the unity editor entering and exiting playmode. So that once we exit, we reset
			// back to the copied list.
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state is not PlayModeStateChange.ExitingPlayMode)
				return;

			if (!Enumerable.Any(maskComponentsCopy))
				return;

			MaskComponents.Clear();
			MaskComponents.AddRange(maskComponentsCopy);
		}
#endif
	}
}
