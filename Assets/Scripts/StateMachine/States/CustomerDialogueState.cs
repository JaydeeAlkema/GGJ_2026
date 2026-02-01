using Customer;
using Dialogue;

namespace StateMachine.States
{
	public class CustomerDialogueState : StateBase
	{
		private bool _canAdvance;

		public override void Enter()
		{
			base.Enter();

			_canAdvance = false;

			CustomerQueueManager.CustomerGreeted += OnCustomerGreeted;
		}

		public override void Tick(float deltaTime)
		{
			if (!_canAdvance)
				return;

			if (DialogueAdvanceInput.IsAdvancePressed())
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
			return StateId.VendorDialogue;
		}

		private void OnCustomerGreeted(Customer.Customer _)
		{
			CustomerQueueManager.CustomerGreeted -= OnCustomerGreeted;
			_canAdvance = true;
		}
	}
}
