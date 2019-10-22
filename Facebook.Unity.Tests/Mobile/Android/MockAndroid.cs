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

namespace Facebook.Unity.Tests.Mobile.Android
{
    using System;
    using System.Collections.Generic;
    using Facebook.Unity.Mobile.Android;
    using NUnit.Framework;

    internal class MockAndroid : MockWrapper, IAndroidWrapper
    {
        public T CallStatic<T>(string methodName)
        {
            if (methodName == "GetSdkVersion")
            {
                object result = "1.0.0";
                return (T)result;
            }
            else if (methodName == "GetUserID")
            {
                object result = "userid";
                return (T)result;
            }

            throw new NotImplementedException();
        }

        public void CallStatic(string methodName, params object[] args)
        {
            this.LogMethodCall(methodName);
            Utilities.Callback<ResultContainer> callback = null;
            IDictionary<string, object> result;
            IDictionary<string, object> methodArguments = null;
            int callbackID = -1;
            if (args.Length == 1)
            {
                var jsonParams = (string)args[0];
                if (jsonParams != null)
                {
                    methodArguments = MiniJSON.Json.Deserialize(jsonParams) as IDictionary<string, object>;
                    string callbackStr;
                    if (methodArguments != null && methodArguments.TryGetValue(Constants.CallbackIdKey, out callbackStr))
                    {
                        callbackID = int.Parse(callbackStr);
                    }
                }
            }

            if (callbackID == -1 && methodName != "Init")
            {
                // There was no callback so just return;
                return;
            }

            if (methodName == "Init")
            {
                callback = this.MobileFacebook.OnInitComplete;
                result = MockResults.GetGenericResult(0, this.ResultExtras);
            }
            else if (methodName == "GetAppLink")
            {
                callback = this.Facebook.OnGetAppLinkComplete;
                result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
            }
            else if (methodName == "AppRequest")
            {
                callback = this.Facebook.OnAppRequestsComplete;
                result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
            }
            else if (methodName == "FeedShare")
            {
                callback = this.Facebook.OnShareLinkComplete;
                result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
            }
            else if (methodName == "ShareLink")
            {
                callback = this.Facebook.OnShareLinkComplete;
                result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
            }
            else if (methodName == "LoginWithPublishPermissions" || methodName == "LoginWithReadPermissions")
            {
                callback = this.Facebook.OnLoginComplete;
                string permissions;
                methodArguments.TryGetValue(AndroidFacebook.LoginPermissionsKey, out permissions);
                result = MockResults.GetLoginResult(
                    callbackID,
                    permissions,
                    this.ResultExtras);
            }
            else if (methodName == "RefreshCurrentAccessToken")
            {
                callback = this.MobileFacebook.OnRefreshCurrentAccessTokenComplete;
                result = MockResults.GetLoginResult(
                    callbackID,
                    string.Empty,
                    this.ResultExtras);
            }
            else
            {
                throw new NotImplementedException("Not implemented for " + methodName);
            }

            callback(new ResultContainer(result));
        }
    }
}
