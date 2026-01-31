namespace StateMachine.States
{
	public abstract class StateBase
	{
		public virtual void Enter()
		{
			IsComplete = false;
		}
		public abstract void Tick(float deltaTime);
		public abstract void Exit();

		public virtual bool IsComplete { get; set; }

		public abstract StateId GetNextStateId();
	}
}
