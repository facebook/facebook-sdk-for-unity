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

namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;

    internal class CallbackManager
    {
        private IDictionary<string, object> facebookDelegates = new Dictionary<string, object>();
        private int nextAsyncId;

        public string AddFacebookDelegate<T>(FacebookDelegate<T> callback) where T : IResult
        {
            if (callback == null)
            {
                return null;
            }

            this.nextAsyncId++;
            this.facebookDelegates.Add(this.nextAsyncId.ToString(), callback);
            return this.nextAsyncId.ToString();
        }

        public void OnFacebookResponse(IInternalResult result)
        {
            if (result == null || result.CallbackId == null)
            {
                return;
            }

            object callback;
            if (this.facebookDelegates.TryGetValue(result.CallbackId, out callback))
            {
                CallCallback(callback, result);
                this.facebookDelegates.Remove(result.CallbackId);
            }
        }

        // Since unity mono doesn't support covariance and contravariance use this hack
        private static void CallCallback(object callback, IResult result)
        {
            if (callback == null || result == null)
            {
                return;
            }

            if (CallbackManager.TryCallCallback<IAppRequestResult>(callback, result) ||
                CallbackManager.TryCallCallback<IShareResult>(callback, result) ||
                CallbackManager.TryCallCallback<IGroupCreateResult>(callback, result) ||
                CallbackManager.TryCallCallback<IGroupJoinResult>(callback, result) ||
                CallbackManager.TryCallCallback<IPayResult>(callback, result) ||
                CallbackManager.TryCallCallback<IAppInviteResult>(callback, result) ||
                CallbackManager.TryCallCallback<IAppLinkResult>(callback, result) ||
                CallbackManager.TryCallCallback<ILoginResult>(callback, result))
            {
                return;
            }

            throw new NotSupportedException("Unexpected result type: " + callback.GetType().FullName);
        }

        private static bool TryCallCallback<T>(object callback, IResult result) where T : IResult
        {
            var castedCallback = callback as FacebookDelegate<T>;
            if (castedCallback != null)
            {
                castedCallback((T)result);
                return true;
            }

            return false;
        }
    }
}
