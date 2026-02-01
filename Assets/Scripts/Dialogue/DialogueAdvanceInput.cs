using UnityEngine.InputSystem;

namespace Dialogue
{
	public static class DialogueAdvanceInput
	{
		public static bool IsAdvancePressed()
		{
			Keyboard keyboard = Keyboard.current;
			if (keyboard == null)
				return false;

			return keyboard.spaceKey.wasPressedThisFrame ||
			       keyboard.enterKey.wasPressedThisFrame ||
			       keyboard.numpadEnterKey.wasPressedThisFrame;
		}
	}
}
