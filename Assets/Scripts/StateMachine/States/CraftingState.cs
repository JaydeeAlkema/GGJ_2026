using Crafting;

namespace StateMachine.States
{
	public class CraftingState : StateBase
	{
		public override void Enter()
		{
			base.Enter();

			CraftingComponentMenu.OnMaskCompleted += CraftingComponentMenu_OnMaskCompleted;
		}

		private void CraftingComponentMenu_OnMaskCompleted()
		{
			CraftingComponentMenu.OnMaskCompleted -= CraftingComponentMenu_OnMaskCompleted;

			this.IsComplete = true;
		}

		public override void Tick(float deltaTime) { }

		public override void Exit()
		{
			CraftingComponentMenu.OnMaskCompleted -= CraftingComponentMenu_OnMaskCompleted;
		}

		public override StateId GetNextStateId()
		{
			return StateId.VendorDialogue;
		}
	}
}
