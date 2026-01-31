namespace StateMachine.States
{
	public class VendorDialogueState : StateBase
	{
		private float counter;

		public override void Enter()
		{
			base.Enter();

			counter = 0;
			this.IsComplete = true;

		}

		public override void Tick(float deltaTime)
		{
			// counter += deltaTime;
			// if (counter >= 3)
			// {
			// }
		}

		public override void Exit() { }

		public override StateId GetNextStateId()
		{
			return StateId.CustomerDialogue;
		}
	}
}
