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
    using MiniJSON;

    internal class MockCanvas : MockWrapper, ICanvasJSWrapper
    {
        internal const string MethodAppRequests = "apprequests";
        internal const string MethodFeed = "feed";
        internal const string MethodPay = "pay";

        public string IntegrationMethodJs
        {
            get
            {
                return "alert(\"MockCanvasTest\");";
            }
        }

        public string GetSDKVersion()
        {
            return "1.0.0";
        }

        public void DisableFullScreen()
        {
            this.LogMethodCall();
        }

        public void Init(string connectFacebookUrl, string locale, int debug, string initParams, int status)
        {
            this.LogMethodCall();

            // Handle testing of init returning access token. It would be nice
            // to not have init return the access token but this could be
            // a breaking change for people who read the raw result
            ResultContainer resultContainer;
            IDictionary<string, object> resultExtras = this.ResultExtras;
            if (resultExtras != null)
            {
                var result = MockResults.GetGenericResult(0, resultExtras);
                resultContainer = new ResultContainer(result);
            }
            else
            {
                resultContainer = new ResultContainer(string.Empty);
            }

            this.Facebook.OnInitComplete(resultContainer);
        }

        public void Login(IEnumerable<string> scope, string callback_id)
        {
            this.LogMethodCall();
            var result = MockResults.GetLoginResult(int.Parse(callback_id), scope.ToCommaSeparateList(), this.ResultExtras);
            this.Facebook.OnLoginComplete(new ResultContainer(result));
        }

        public void Logout()
        {
            this.LogMethodCall();
        }

        public void ActivateApp()
        {
            this.LogMethodCall();
        }

        public void LogAppEvent(string eventName, float? valueToSum, string parameters)
        {
            this.LogMethodCall();
        }

        public void LogPurchase(float purchaseAmount, string currency, string parameters)
        {
            this.LogMethodCall();
        }

        public void Ui(string x, string uid, string callbackMethodName)
        {
            this.LogMethodCall();
            int cbid = Convert.ToInt32(uid);
            var methodArguments = Json.Deserialize(x) as IDictionary<string, object>;

            string methodName;
            if (null != methodArguments &&
                methodArguments.TryGetValue<string>("method", out methodName))
            {
                if (methodName.Equals(MethodAppRequests))
                {
                    var result = MockResults.GetGenericResult(cbid, this.ResultExtras);
                    this.Facebook.OnAppRequestsComplete(new ResultContainer(result));
                }
                else if (methodName.Equals(MethodFeed))
                {
                    var result = MockResults.GetGenericResult(cbid, this.ResultExtras);
                    this.Facebook.OnShareLinkComplete(new ResultContainer(result));
                }
                else if (methodName.Equals(MethodPay))
                {
                    var result = MockResults.GetGenericResult(cbid, this.ResultExtras);
                    this.CanvasFacebook.OnPayComplete(new ResultContainer(result));
                }
            }
        }

        public void InitScreenPosition()
        {
            this.LogMethodCall();
        }
    }
}
