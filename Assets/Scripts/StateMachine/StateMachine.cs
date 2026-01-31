using System;
using System.Collections.Generic;
using StateMachine.States;
using UnityEngine;

namespace StateMachine
{
	public class StateMachine : MonoBehaviour
	{
		public static event Action<StateBase, StateBase> OnStateChanged;

		public StateBase CurrentState { get; private set; }

		private Dictionary<StateId, StateBase> states;

		private void Awake()
		{
			states = new Dictionary<StateId, StateBase>
			{
				{
					StateId.Intro, new IntroState()
				},
				{
					StateId.VendorDialogue, new VendorDialogueState()
				},
				{
					StateId.CustomerDialogue, new CustomerDialogueState()
				},
				{
					StateId.Crafting, new CraftingState()
				},
			};
		}

		public void Start()
		{
			SetState(new IntroState());
		}

		private void Update()
		{
			if (CurrentState == null)
				return;

			CurrentState.Tick(Time.deltaTime);

			if (CurrentState.IsComplete)
			{
				AdvanceState();
			}
		}

		private void AdvanceState()
		{
			SetState(states[CurrentState.GetNextStateId()]);
		}

		private void SetState(StateBase newState)
		{
			StateBase previous = CurrentState;

			CurrentState?.Exit();

			newState.IsComplete = false;
			CurrentState = newState;

			CurrentState.Enter();

			OnStateChanged?.Invoke(previous, CurrentState);
		}
	}
}
