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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
    public class CodelessUIInteractEvent : MonoBehaviour
    {
        private FBSDKEventBindingManager eventBindingManager { get; set; }

        void Awake ()
        {
            switch (Constants.CurrentPlatform) {
            case FacebookUnityPlatform.Android:
                SetLoggerInitAndroid ();
                break;
            case FacebookUnityPlatform.IOS:
                SetLoggerInitIos ();
                break;
            default:
                break;
            }
        }

        private static void SetLoggerInitAndroid ()
        {
            AndroidJavaObject fetchedAppSettingsManager = new AndroidJavaClass ("com.facebook.internal.FetchedAppSettingsManager");
            fetchedAppSettingsManager.CallStatic ("setIsUnityInit", true);
        }

        private static void SetLoggerInitIos ()
        {
            //PLACEHOLDER for IOS
        }

        public void OnReceiveMapping (string message)
        {
            var dict = MiniJSON.Json.Deserialize(message) as List<System.Object>;
            this.eventBindingManager = new FBSDKEventBindingManager(dict);
        }

    }
}
