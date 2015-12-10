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
        private Rect modalRect;
        private GUIStyle modalStyle;

        public EditorFacebookMockDialog()
        {
            this.modalRect = new Rect(10, 10, Screen.width - 20, Screen.height - 20);
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1.0f));
            texture.Apply();
            this.modalStyle = new GUIStyle(GUI.skin.window);
            this.modalStyle.normal.background = texture;
        }

        public delegate void OnComplete(string result);

        public OnComplete Callback { protected get; set; }

        public string CallbackID { protected get; set; }

        protected abstract string DialogTitle { get; }

        public void OnGUI()
        {
            GUI.ModalWindow(
                this.GetHashCode(),
                this.modalRect,
                this.OnGUIDialog,
                this.DialogTitle,
                this.modalStyle);
        }

        protected abstract void DoGui();

        protected abstract void SendSuccessResult();

        protected virtual void SendCancelResult()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary[Constants.CancelledKey] = true;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(MiniJSON.Json.Serialize(dictionary));
        }

        protected virtual void SendErrorResult(string errorMessage)
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
            var cancelButtonRect = GUILayoutUtility.GetRect(cancelLabel, GUI.skin.button);
            if (GUI.Button(cancelButtonRect, cancelLabel, GUI.skin.button))
            {
                this.SendCancelResult();
                MonoBehaviour.Destroy(this);
            }

            var errorLabel = new GUIContent("Send Error");
            var errorButtonRect = GUILayoutUtility.GetRect(cancelLabel, GUI.skin.button);
            if (GUI.Button(errorButtonRect, errorLabel, GUI.skin.button))
            {
                this.SendErrorResult("Error: Error button pressed");
                MonoBehaviour.Destroy(this);
            }

            GUILayout.EndHorizontal();
        }
    }
}
