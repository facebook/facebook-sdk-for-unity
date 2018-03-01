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

namespace Facebook.Unity.Canvas
{
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using Facebook.Unity.Canvas.Webgl;

    internal class CanvasWrapper : ICanvasWrapper
    {
        public void Init(string connectFacebookUrl, string locale, int debug, string initParams, int status)
        {
            CanvasWrapper.init(connectFacebookUrl, locale, debug, initParams, status);
        }

        public void Login(IEnumerable<string> scope, string callback_id)
        {
            CanvasWrapper.login(scope, callback_id);
        }

        public void LogOut()
        {
            CanvasWrapper.logout();
        }

        public void ActivateApp()
        {
            CanvasWrapper.activateApp();
        }

        public void LogAppEvent(string eventName, float? valueToSum, string parameters)
        {
            CanvasWrapper.logAppEvent(eventName, valueToSum, parameters);
        }

        public void LogPurchase(float purchaseAmount, string currency, string parameters)
        {
            CanvasWrapper.logPurchase(purchaseAmount, currency, parameters);
        }

        public void UI(string x, string uid, string callbackMethodName)
        {
            CanvasWrapper.ui(x, uid, callbackMethodName);
        }

        public void InitScreenPosition()
        {
            CanvasWrapper.initScreenPosition();
        }

        [DllImport("__Internal")]
        private static extern void init(string connectFacebookUrl, string locale, int debug, string initParams, int status);

        [DllImport("__Internal")]
        private static extern void login(IEnumerable<string> scope, string callback_id);

        [DllImport("__Internal")]
        private static extern void logout();

        [DllImport("__Internal")]
        private static extern void activateApp();

        [DllImport("__Internal")]
        private static extern void logAppEvent(string eventName, float? valueToSum, string parameters);

        [DllImport("__Internal")]
        private static extern void logPurchase(float purchaseAmount, string currency, string parameters);

        [DllImport("__Internal")]
        private static extern void ui(string x, string uid, string callbackMethodName);

        [DllImport("__Internal")]
        private static extern void initScreenPosition();
    }
}
