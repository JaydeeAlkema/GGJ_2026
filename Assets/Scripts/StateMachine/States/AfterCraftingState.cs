using Customer;

namespace StateMachine.States
{
	public class AfterCraftingState : StateBase
	{
		public override void Enter()
		{
			base.Enter();

			CustomerSatisfactionBar.OnScoreBarVisualsUpdated += CustomerSatisfactionBar_OnScoreBarVisualsUpdated;
		}

		public override void Tick(float deltaTime) { }

		public override void Exit()
		{
			CustomerSatisfactionBar.OnScoreBarVisualsUpdated -= CustomerSatisfactionBar_OnScoreBarVisualsUpdated;
		}

		public override StateId GetNextStateId()
		{
			return StateId.CustomerDialogue;
		}

		private void CustomerSatisfactionBar_OnScoreBarVisualsUpdated()
		{
			CustomerSatisfactionBar.OnScoreBarVisualsUpdated -= CustomerSatisfactionBar_OnScoreBarVisualsUpdated;

			this.IsComplete = true;
		}
	}
}
