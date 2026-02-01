using Dialogue;
using UnityEngine;

namespace StateMachine.States
{
	public class VendorDialogueState : StateBase
	{
		private VendorDialogue _vendorDialogue;
		private bool _canAdvance;

		public override void Enter()
		{
			base.Enter();

			_canAdvance = true;
			_vendorDialogue = Object.FindFirstObjectByType<VendorDialogue>();
			_vendorDialogue?.Speak();
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
			_vendorDialogue?.Hide();
		}

		public override StateId GetNextStateId()
		{
			return StateId.Crafting;
		}
	}
}
