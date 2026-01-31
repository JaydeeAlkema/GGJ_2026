using Crafting;
using UnityEngine;

namespace StateMachine.States
{
	public class CraftingState : StateBase
	{
		public override void Enter()
		{
			base.Enter();

			Debug.Log("Entering Crafting State");

			CraftingComponentMenu.OnMaskCompleted += CraftingComponentMenu_OnMaskCompleted;
		}

		private void CraftingComponentMenu_OnMaskCompleted()
		{
			CraftingComponentMenu.OnMaskCompleted -= CraftingComponentMenu_OnMaskCompleted;

			Debug.Log("Mask crafting completed.");

			this.IsComplete = true;
		}

		public override void Tick(float deltaTime) { }

		public override void Exit()
		{
			Debug.Log("Exiting Crafting State");

			CraftingComponentMenu.OnMaskCompleted -= CraftingComponentMenu_OnMaskCompleted;
		}
		public override StateId GetNextStateId()
		{
			return StateId.VendorDialogue;
		}
	}
}
