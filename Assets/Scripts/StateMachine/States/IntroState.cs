namespace StateMachine.States
{
	public class IntroState : StateBase
	{
		private float counter;

		public override void Enter()
		{
			base.Enter();

			counter = 0;
		}

		public override void Tick(float deltaTime)
		{
			counter += deltaTime;
			if (counter >= 1f)
			{
				this.IsComplete = true;
			}
		}

		public override void Exit() { }

		public override StateId GetNextStateId()
		{
			return StateId.VendorDialogue;
		}
	}
}
