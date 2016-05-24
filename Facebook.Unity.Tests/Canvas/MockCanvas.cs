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

namespace Facebook.Unity.Tests.Canvas
{
    using System;
    using System.Collections.Generic;
    using Facebook.Unity.Canvas;

    internal class MockCanvas : MockWrapper, ICanvasJSWrapper
    {
        public string IntegrationMethodJs
        {
            get
            {
                return null;
            }
        }

        public string GetSDKVersion()
        {
            return "1.0.0";
        }

        public void ExternalCall(string functionName, object[] args)
        {
            this.LogMethodCall(functionName);
            IDictionary<string, object> result;
            Utilities.Callback<ResultContainer> callback = null;

            if (functionName == "FBUnity.logAppEvent")
            {
                // Workaround log the method call to match the signature of ios and android.
                this.LogMethodCall("LogAppEvent");

                // No callback on log app event
                return;
            }
            else if (functionName == "FBUnity.login")
            {
                var permissions = (IEnumerable<string>)args[0];
                var callbackID = int.Parse((string)args[1]);
                result = MockResults.GetLoginResult(callbackID, permissions, this.ResultExtras);
                callback = this.Facebook.OnLoginComplete;
            }
            else if (functionName == "FBUnity.ui")
            {
                var callbackMetod = (string)args[2];
                var callbackID = int.Parse((string)args[1]);

                if (callbackMetod == Constants.OnGroupCreateCompleteMethodName)
                {
                    result = MockResults.GetGroupCreateResult(callbackID, this.ResultExtras);
                    callback = this.Facebook.OnGroupCreateComplete;
                }
                else if (callbackMetod == Constants.OnGroupJoinCompleteMethodName)
                {
                    result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
                    callback = this.Facebook.OnGroupJoinComplete;
                }
                else if (callbackMetod == Constants.OnAppRequestsCompleteMethodName)
                {
                    result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
                    callback = this.Facebook.OnAppRequestsComplete;
                }
                else if (callbackMetod == Constants.OnShareCompleteMethodName)
                {
                    result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
                    callback = this.Facebook.OnShareLinkComplete;
                }
                else if (callbackMetod == Constants.OnPayCompleteMethodName)
                {
                    result = MockResults.GetGenericResult(callbackID, this.ResultExtras);
                    callback = this.CanvasFacebook.OnPayComplete;
                }
                else
                {
                    throw new NotImplementedException("Mock missing ui function: " + callbackMetod);
                }
            }
            else
            {
                throw new NotImplementedException("Mock missing function: " + functionName);
            }

            callback(new ResultContainer(result));
        }

        public void ExternalEval(string script)
        {
            // noop
        }

        public void DisableFullScreen()
        {
            // noop
        }
    }
}
