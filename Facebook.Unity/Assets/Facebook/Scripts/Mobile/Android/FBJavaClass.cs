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

namespace Facebook.Unity.Mobile.Android
{
    using System;
    using UnityEngine;

    internal class FBJavaClass : IAndroidJavaClass
    {
        private const string FacebookJavaClassName = "com.facebook.unity.FB";
        private AndroidJavaClass facebookJavaClass = new AndroidJavaClass(FacebookJavaClassName);

        public T CallStatic<T>(string methodName)
        {
            return this.facebookJavaClass.CallStatic<T>(methodName);
        }

        public void CallStatic(string methodName, params object[] args)
        {
            this.facebookJavaClass.CallStatic(methodName, args);
        }

        // Mock the AndroidJava to compile on other platforms
        #if !UNITY_ANDROID
        private class AndroidJNIHelper
        {
            public static bool Debug { get; set; }
        }

        private class AndroidJavaClass
        {
            public AndroidJavaClass(string mock)
            {
            }

            public T CallStatic<T>(string method)
            {
                return default(T);
            }

            public void CallStatic(string method, params object[] args)
            {
            }
        }
        #endif
    }
}
