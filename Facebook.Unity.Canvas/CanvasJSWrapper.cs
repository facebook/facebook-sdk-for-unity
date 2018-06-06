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
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class CanvasJSWrapper : ICanvasJSWrapper
    {
        public string GetSDKVersion()
        {
            return Constants.GraphApiVersion;
        }

        public void DisableFullScreen()
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
        }

        public void Init(string connectFacebookUrl, string locale, int debug, string initParams, int status)
        {
            init(connectFacebookUrl, locale, debug, initParams, status);
        }

        public void Login(IEnumerable<string> scope, string callback_id)
        {
            login(MiniJSON.Json.Serialize(scope), callback_id);
        }

        public void Logout() {
            logout();
        }

        public void ActivateApp() {
            activateApp();
        }

        public void LogAppEvent(string eventName, float? valueToSum, string parameters) {
            logAppEvent(eventName, valueToSum.GetValueOrDefault(), parameters);
        }

        public void LogPurchase(float purchaseAmount, string currency, string parameters) {
            logPurchase(purchaseAmount, currency, parameters);
        }

        public void Ui(string x, string uid, string callbackMethodName) {
            ui(x, uid, callbackMethodName);
        }

        public void InitScreenPosition() {
            initScreenPosition();
        }

        [DllImport("__Internal")]
        private static extern void init(string connectFacebookUrl, string locale, int debug, string initParams, int status);

        [DllImport("__Internal")]
        private static extern void login(string scope, string callback_id);

        [DllImport("__Internal")]
        private static extern void logout();

        [DllImport("__Internal")]
        private static extern void activateApp();

        [DllImport("__Internal")]
        private static extern void logAppEvent(string eventName, float valueToSum, string parameters);

        [DllImport("__Internal")]
        private static extern void logPurchase(float purchaseAmount, string currency, string parameters);

        [DllImport("__Internal")]
        private static extern void ui(string x, string uid, string callbackMethodName);

        [DllImport("__Internal")]
        private static extern void initScreenPosition();
    }
}
