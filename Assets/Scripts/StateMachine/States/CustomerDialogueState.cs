namespace StateMachine.States
{
	public class CustomerDialogueState : StateBase
	{
		public override void Enter()
		{
			base.Enter();

			// Test to skip this state.
			this.IsComplete = true;
		}

		public override void Tick(float deltaTime) { }

		public override void Exit() { }

		public override StateId GetNextStateId()
		{
			return StateId.Crafting;
		}
	}
}
