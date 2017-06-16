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

namespace Facebook.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Gameroom;
    using Facebook.Unity.Mobile;

    internal class MockWrapper
    {
        private IDictionary<string, object> resultExtras;

        private IDictionary<string, int> methodCallCounts = new Dictionary<string, int>();

        // These extras are cleared on first access to avoid appending to
        // multiple calls
        public IDictionary<string, object> ResultExtras
        {
            get
            {
                var result = this.resultExtras;
                this.resultExtras = null;
                return result;
            }

            set
            {
                this.resultExtras = value;
            }
        }

        internal IFacebookResultHandler Facebook { get; set; }

        internal ICanvasFacebookResultHandler CanvasFacebook
        {
            get
            {
                return this.Facebook as ICanvasFacebookResultHandler;
            }
        }

        internal IMobileFacebookResultHandler MobileFacebook
        {
            get
            {
                return this.Facebook as IMobileFacebookResultHandler;
            }
        }

        internal IGameroomFacebookResultHandler GameroomFacebook
        {
            get
            {
                return this.Facebook as IGameroomFacebookResultHandler;
            }
        }

        public int GetMethodCallCount(string methodName)
        {
            int count;
            if (this.methodCallCounts.TryGetValue(methodName, out count))
            {
                return count;
            }

            return 0;
        }

        protected IDictionary<string, object> GetResultDictionary(int requestId)
        {
            var result = new Dictionary<string, object>()
            {
                { Constants.CallbackIdKey, requestId.ToString() }
            };

            var extras = this.ResultExtras;
            if (extras != null)
            {
                result.AddAllKVPFrom(extras);
            }

            return result;
        }

        protected void LogMethodCall()
        {
            var st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            string methodName = sf.GetMethod().Name;
            this.LogMethodCall(methodName);
        }

        protected void LogMethodCall(string methodName)
        {
            int count;
            if (this.methodCallCounts.TryGetValue(methodName, out count))
            {
                this.methodCallCounts[methodName] = ++count;
            }
            else
            {
                this.methodCallCounts[methodName] = 1;
            }
        }
    }
}
