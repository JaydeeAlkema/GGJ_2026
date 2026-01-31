using System.Collections.Generic;
using System.Linq;
using Mask;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Inventory
{
	[CreateAssetMenu(fileName = "Completed Mask Items SO", menuName = "ScriptableObjects/new Completed Mask Items SO", order = 0)]
	public class CompletedMaskItemsSO : ScriptableObject
	{
		private readonly List<CompletedMaskItem> completedMaskItems = new();

		public void AddCompletedMaskItem(CompletedMaskItem completedMaskItem)
		{
			completedMaskItems.Add(completedMaskItem);
		}

		public void RemoveCompletedMaskItem(CompletedMaskItem completedMaskItem)
		{
			completedMaskItems.Remove(completedMaskItem);
		}

		public List<CompletedMaskItem> GetCompletedMaskItems()
		{
			return completedMaskItems;
		}

		// Editor-only code to maintain state of the MaskComponents list during playmode.
		// This is necessary because ScriptableObjects retain their state changes made during playmode,
		// which can lead to unintended side effects in the editor once playmode is exited.
		// Is this ideal? Hell no, but it works for now. ~Jaydee
#if UNITY_EDITOR
		private readonly List<CompletedMaskItem> completedMaskItemsCopy = new();

		private void OnValidate()
		{
			completedMaskItemsCopy.Clear();
			completedMaskItemsCopy.AddRange(completedMaskItems);

			// subscribe to the unity editor entering and exiting playmode. So that once we exit, we reset back to the copied list. ~Jaydee
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state is not PlayModeStateChange.ExitingPlayMode)
				return;

			if (!completedMaskItemsCopy.Any())
				return;

			completedMaskItems.Clear();
			completedMaskItems.AddRange(completedMaskItemsCopy);
		}
#endif
	}
}
