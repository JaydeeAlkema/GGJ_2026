using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Goal
{
	public class GoalManager : MonoBehaviour
	{
		[BoxGroup("Goal Settings")]
		[SerializeField] private int TotalGoldRequired = 100;

		[BoxGroup("References")]
		[SerializeField] private Image GoalProgressBar;

		[BoxGroup("References")]
		[SerializeField] private CutsceneManager CutsceneManager;

		private int _currentGold;

		private void OnEnable()
		{
			SetBarFilledAmount();
		}

		public void AddGold(int amount)
		{
			_currentGold += amount;
			SetBarFilledAmount();

			if (!IsGoalAchieved())
				return;

			CutsceneManager.TransitionToScene("YouWin");
		}

		private void SetBarFilledAmount()
		{
			float fillAmount = _currentGold / (float)TotalGoldRequired;
			GoalProgressBar.fillAmount = fillAmount;
		}

		public bool IsGoalAchieved()
		{
			return _currentGold >= TotalGoldRequired;
		}
	}
}
