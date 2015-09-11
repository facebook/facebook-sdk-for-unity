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
    using Facebook.Unity.Mobile.Android;
    using NUnit.Framework;

    internal class MockAndroidJavaClass : IAndroidJavaClass
    {
        private object callbackStaticMethod;

        public delegate void OnResult(string result);

        public OnResult Callback { get; set; }

        public string Result { get; set; }

        public T CallStatic<T>(string methodName)
        {
            if (this.callbackStaticMethod == null)
            {
                return default(T);
            }

            var method = (Func<string, T>)this.callbackStaticMethod;
            return method(methodName);
        }

        public void CallStatic(string methodName, params object[] args)
        {
            Assert.IsFalse(string.IsNullOrEmpty(methodName));
            Assert.IsFalse(string.IsNullOrEmpty((string)args[0]));
            this.Callback(this.Result);
        }

        public void SetMethodReplacement<T>(Func<string, T> method)
        {
            this.callbackStaticMethod = method;
        }
    }
}
