using Customer;

namespace StateMachine.States
{
	public class CustomerDialogueState : StateBase
	{
		private float _counter = 3f;
		private bool _canCountDown;

		public override void Enter()
		{
			base.Enter();

			_counter = 3f;
			_canCountDown = false;

			CustomerQueueManager.CustomerGreeted += OnCustomerGreeted;
		}

		public override void Tick(float deltaTime)
		{
			if (!_canCountDown)
				return;

			_counter -= deltaTime;
			if (_counter <= 0f)
			{
				this.IsComplete = true;
			}
		}

		public override void Exit()
		{
			CustomerQueueManager.CustomerGreeted -= OnCustomerGreeted;
		}

		public override StateId GetNextStateId()
		{
			return StateId.Crafting;
		}

		private void OnCustomerGreeted(Customer.Customer _)
		{
			CustomerQueueManager.CustomerGreeted -= OnCustomerGreeted;
			_canCountDown = true;
		}
	}
}
