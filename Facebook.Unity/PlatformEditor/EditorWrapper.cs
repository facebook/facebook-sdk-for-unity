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
    using Facebook.Unity.Editor.Dialogs;

    internal class EditorWrapper : IEditorWrapper
    {
        private IFacebookCallbackHandler callbackHandler;

        public EditorWrapper(IFacebookCallbackHandler callbackHandler)
        {
            this.callbackHandler = callbackHandler;
        }

        public void Init()
        {
            this.callbackHandler.OnInitComplete(string.Empty);
        }

        public void ShowLoginMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId,
            string permsisions)
        {
            var dialog = ComponentFactory.GetComponent<MockLoginDialog>();
            dialog.Callback = callback;
            dialog.CallbackID = callbackId;
        }

        public void ShowAppRequestMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            this.ShowEmptyMockDialog(callback, callbackId, "Mock App Request");
        }

        public void ShowGameGroupCreateMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            this.ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Create");
        }

        public void ShowGameGroupJoinMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            this.ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Join");
        }

        public void ShowAppInviteMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            this.ShowEmptyMockDialog(callback, callbackId, "Mock App Invite");
        }

        public void ShowPayMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            this.ShowEmptyMockDialog(callback, callbackId, "Mock Pay");
        }

        public void ShowMockShareDialog(
            Utilities.Callback<ResultContainer> callback,
            string subTitle,
            string callbackId)
        {
            var dialog = ComponentFactory.GetComponent<MockShareDialog>();
            dialog.SubTitle = subTitle;
            dialog.Callback = callback;
            dialog.CallbackID = callbackId;
        }

        private void ShowEmptyMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId,
            string title)
        {
            var dialog = ComponentFactory.GetComponent<EmptyMockDialog>();
            dialog.Callback = callback;
            dialog.CallbackID = callbackId;
            dialog.EmptyDialogTitle = title;
        }
    }
}
