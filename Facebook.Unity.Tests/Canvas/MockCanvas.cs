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
    using Facebook.Unity.Canvas.Webgl;

    internal class MockCanvas : MockWrapper, ICanvasJSWrapper, ICanvasWrapper
    {
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
            // noop
        }

        public void Init(string connectFacebookUrl, string locale, int debug, string initParams, int status)
        {
            // noop
        }
        
        public void Login(string scope, string callback_id)
        {
            // noop
        }

        public void LogOut()
        {
            // noop
        }

        public void ActivateApp()
        {
            // noop
        }

        public void LogAppEvent(string eventName, float? valueToSum, string parameters)
        {
            // noop
        }

        public void LogPurchase(float purchaseAmount, string currency, string parameters)
        {
            // noop
        }

        public void UI(string x, string uid, string callbackMethodName)
        {
            // noop
        }

        public void InitScreenPosition()
        {
            // noop
        }
    }
}
