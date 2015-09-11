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

namespace Facebook.Unity.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal abstract class EditorFacebookMockDialog : MonoBehaviour
    {
        private float windowHeight = 200;
        private GUIStyle greyButton;

        public delegate void OnComplete(string result);

        public OnComplete Callback { protected get; set; }

        public string CallbackID { protected get; set; }

        protected abstract float WindowHeight { get; }

        protected abstract string DialogTitle { get; }

        public void OnGUI()
        {
            var windowTop = (Screen.height / 2) - (this.WindowHeight / 2);
            var windowLeft = (Screen.width / 2) - (this.WindowHeight / 2);
            this.greyButton = GUI.skin.button;
            GUI.ModalWindow(
                this.GetHashCode(),
                new Rect(windowLeft, windowTop, this.WindowHeight, this.windowHeight),
                this.OnGUIDialog,
                this.DialogTitle);
        }

        protected abstract void DoGui();

        protected abstract void SendSuccessResult();

        protected void SendCancelResult()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary[Constants.CancelledKey] = true;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(MiniJSON.Json.Serialize(dictionary));
        }

        protected void SendErrorResult(string errorMessage)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary[Constants.ErrorKey] = errorMessage;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(MiniJSON.Json.Serialize(dictionary));
        }

        private void OnGUIDialog(int windowId)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs");
            GUILayout.Label("Test your app on one of the supported platforms");
            this.DoGui();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var loginLabel = new GUIContent("Send Success");
            var buttonRect = GUILayoutUtility.GetRect(loginLabel, GUI.skin.button);
            if (GUI.Button(buttonRect, loginLabel))
            {
                this.SendSuccessResult();
                MonoBehaviour.Destroy(this);
            }

            var cancelLabel = new GUIContent("Send Cancel");
            var cancelButtonRect = GUILayoutUtility.GetRect(cancelLabel, this.greyButton);
            if (GUI.Button(cancelButtonRect, cancelLabel, this.greyButton))
            {
                this.SendCancelResult();
                MonoBehaviour.Destroy(this);
            }

            var errorLabel = new GUIContent("Send Error");
            var errorButtonRect = GUILayoutUtility.GetRect(cancelLabel, this.greyButton);
            if (GUI.Button(errorButtonRect, errorLabel, this.greyButton))
            {
                this.SendErrorResult("Error: Error button pressed");
                MonoBehaviour.Destroy(this);
            }

            GUILayout.EndHorizontal();

            if (Event.current.type == EventType.Repaint)
            {
                this.windowHeight = cancelButtonRect.y + cancelButtonRect.height + GUI.skin.window.padding.bottom;
            }
        }
    }
}
