/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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
