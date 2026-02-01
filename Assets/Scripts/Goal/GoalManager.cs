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

		private int _currentGold;

		public void AddGold(int amount)
		{
			_currentGold += amount;
			_currentGold = Mathf.Min(_currentGold, TotalGoldRequired);
			Debug.Log($"Current Gold: {_currentGold}/{TotalGoldRequired}");
		}

		public bool IsGoalAchieved()
		{
			return _currentGold >= TotalGoldRequired;
		}
	}
}
