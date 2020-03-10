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

namespace Facebook.Unity.Tests.Editor
{
    using System;
    using System.Collections.Generic;
    using Facebook.Unity.Editor;

    internal class MockEditor : MockWrapper, IEditorWrapper
    {
        public void Init()
        {
            IDictionary<string, object> resultExtra = this.ResultExtras;
            if (resultExtra != null)
            {
                this.Facebook.OnInitComplete(
                    new ResultContainer(MockResults.GetGenericResult(0, resultExtra)));
            }
            else
            {
                this.Facebook.OnInitComplete(
                    new ResultContainer(string.Empty));
            }
        }

        public void ShowLoginMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId,
            string permissions)
        {
            var result = MockResults.GetLoginResult(int.Parse(callbackId), permissions, this.ResultExtras);
            callback(new ResultContainer(result));
        }

        public void ShowAppRequestMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            var result = MockResults.GetGenericResult(int.Parse(callbackId), this.ResultExtras);
            callback(new ResultContainer(result));
        }

        public void ShowGameGroupCreateMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            var result = MockResults.GetGroupCreateResult(int.Parse(callbackId), this.ResultExtras);
            callback(new ResultContainer(result));
        }

        public void ShowGameGroupJoinMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            var result = MockResults.GetGenericResult(int.Parse(callbackId), this.ResultExtras);
            callback(new ResultContainer(result));
        }

        public void ShowPayMockDialog(
            Utilities.Callback<ResultContainer> callback,
            string callbackId)
        {
            var result = MockResults.GetGenericResult(int.Parse(callbackId), this.ResultExtras);
            callback(new ResultContainer(result));
        }

        public void ShowMockShareDialog(
            Utilities.Callback<ResultContainer> callback,
            string subTitle,
            string callbackId)
        {
            var result = MockResults.GetGenericResult(int.Parse(callbackId), this.ResultExtras);
            callback(new ResultContainer(result));
        }

        public void ShowMockFriendFinderDialog(
            Utilities.Callback<ResultContainer> callback,
            string subTitle,
            string callbackId)
        {
            var result = MockResults.GetGenericResult(int.Parse(callbackId), this.ResultExtras);
            callback(new ResultContainer(result));
        }
    }
}
