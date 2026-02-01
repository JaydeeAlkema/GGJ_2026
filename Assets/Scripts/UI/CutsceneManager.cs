using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
	[SerializeField] private SceneTransition[] transitions;
	[SerializeField] private Fade fade;
	[SerializeField] private float fadeOutDelay = 1f;

	private bool isTransitioning;

	// Update is called once per frame
	private void Update()
	{
		if (isTransitioning || transitions == null || transitions.Length == 0)
		{
			return;
		}

		for (int i = 0; i < transitions.Length; i++)
		{
			SceneTransition transition = transitions[i];
			if (transition == null || transition.keys == null || transition.keys.Length == 0)
			{
				continue;
			}

			for (int k = 0; k < transition.keys.Length; k++)
			{
				if (Keyboard.current != null && Keyboard.current[transition.keys[k]].wasPressedThisFrame)
				{
					TransitionToScene(transition.sceneName);
					return;
				}
			}
		}
	}

	public void TransitionToScene(string sceneName)
	{
		StartCoroutine(TransitionToSceneCo(sceneName));
	}

	private IEnumerator TransitionToSceneCo(string sceneName)
	{
		if (string.IsNullOrWhiteSpace(sceneName))
		{
			yield break;
		}

		isTransitioning = true;

		if (fade != null)
		{
			fade.FadeOut();
			if (fadeOutDelay > 0f)
			{
				yield return new WaitForSeconds(fadeOutDelay);
			}
		}

		SceneManager.LoadScene(sceneName);
	}

	[Serializable]
	private class SceneTransition
	{
		public string sceneName;
		public Key[] keys;
	}
}
