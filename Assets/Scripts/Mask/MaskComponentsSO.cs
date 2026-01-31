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
			return Enumerable.ToList(Enumerable.Cast<IMaskComponent>(Enumerable.Select(MaskComponents, item => item)));
		}

		// Editor-only code to maintain state of the MaskComponents list during playmode.
		// This is necessary because ScriptableObjects retain their state changes made during playmode,
		// which can lead to unintended side effects in the editor once playmode is exited.
		// Is this ideal? Hell no, but it works for now. ~Jaydee
#if UNITY_EDITOR
		private readonly List<MaskComponent> maskComponentsCopy = new();

		private void OnValidate()
		{
			maskComponentsCopy.Clear();
			maskComponentsCopy.AddRange(MaskComponents);

			// subscribe to the unity editor entering and exiting playmode. So that once we exit, we reset back to the copied list. ~Jaydee
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
