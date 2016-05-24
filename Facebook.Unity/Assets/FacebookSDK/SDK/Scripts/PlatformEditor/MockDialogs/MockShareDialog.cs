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

namespace Facebook.Unity.Editor.Dialogs
{
    using System.Collections.Generic;
    using System.Text;

    internal class MockShareDialog : EditorFacebookMockDialog
    {
        public string SubTitle { private get; set; }

        protected override string DialogTitle
        {
            get
            {
                return "Mock " + this.SubTitle + " Dialog";
            }
        }

        protected override void DoGui()
        {
            // Empty
        }

        protected override void SendSuccessResult()
        {
            var result = new Dictionary<string, object>();

            if (FB.IsLoggedIn)
            {
                result["postId"] = this.GenerateFakePostID();
            }
            else
            {
                result["did_complete"] = true;
            }

            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                result[Constants.CallbackIdKey] = this.CallbackID;
            }

            if (this.Callback != null)
            {
                this.Callback(new ResultContainer(result));
            }
        }

        protected override void SendCancelResult()
        {
            var result = new Dictionary<string, object>();
            result[Constants.CancelledKey] = "true";
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                result[Constants.CallbackIdKey] = this.CallbackID;
            }

            this.Callback(new ResultContainer(result));
        }

        private string GenerateFakePostID()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(AccessToken.CurrentAccessToken.UserId);
            sb.Append('_');
            for (int i = 0; i < 17; i++)
            {
                sb.Append(UnityEngine.Random.Range(0, 10));
            }

            return sb.ToString();
        }
    }
}
