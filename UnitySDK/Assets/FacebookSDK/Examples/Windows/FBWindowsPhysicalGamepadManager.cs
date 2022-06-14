using UnityEngine;
using UnityEngine.UI;

#if (UNITY_STANDALONE_WIN || UNTIY_EDITOR_WIN) && !UNITY_WEBGL
    using XInputDotNetPure;
#endif

public class FBWindowsPhysicalGamepadManager : MonoBehaviour {
    public Text displayGamepadInputText;

#if (UNITY_STANDALONE_WIN || UNTIY_EDITOR_WIN) && !UNITY_WEBGL
    public GamePadState state;
    void Update()
    {
        state = GamePad.GetState(0);
        displayGamepadInputText.text = "";
        displayGamepadInputText.text += "--- Sticks ---\n";
        displayGamepadInputText.text += string.Format("Left X: {0}\nLeft Y: {1}\nRight X: {2}\nRight Y: {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        displayGamepadInputText.text += "\n--- Buttons ---\n";
        displayGamepadInputText.text += string.Format("Start: {0} | Back: {1}\n", state.Buttons.Start, state.Buttons.Back);
        displayGamepadInputText.text += string.Format("A: {0} | B: {1}  | X: {2} | Y: {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
        displayGamepadInputText.text += string.Format("LeftShoulder: {0} | RightShoulder: {1}\n", state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
        displayGamepadInputText.text += string.Format("LeftStick: {0} | RightStick: {1}\n", state.Buttons.LeftStick, state.Buttons.RightStick);
        displayGamepadInputText.text += "\n--- Triggers ---\n";
        displayGamepadInputText.text += string.Format("Left Trigger: {0}\nRightTrigger: {1}\n", state.Triggers.Left, state.Triggers.Right);
        displayGamepadInputText.text += "\n--- D-Pad ---\n";
        displayGamepadInputText.text += string.Format("Up: {0} | Right: {1} | Down: {2} | Left: {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
    }
#else
    void Start()
    {
        displayGamepadInputText.text = "Run this example in a Windows device.";
    }
#endif
}
