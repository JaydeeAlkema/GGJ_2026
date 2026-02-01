namespace StateMachine.States
{
	public class VendorDialogueState : StateBase
	{
		public override void Enter()
		{
			base.Enter();

			this.IsComplete = true;
		}

		public override void Tick(float deltaTime) { }

		public override void Exit() { }

		public override StateId GetNextStateId()
		{
			return StateId.CustomerDialogue;
		}
	}
}
