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
    using System.Collections.Generic;
    using Facebook.Unity.Mobile.Android;
    using NUnit.Framework;

    internal class AndroidTest<T> where T : IResult
    {
        private readonly MockAndroidJavaClass mock;
        private readonly CallbackManager callbackManager;

        public AndroidTest()
        {
            this.mock = new MockAndroidJavaClass();
            this.callbackManager = new CallbackManager();
            this.AndroidFacebook = new AndroidFacebook(this.mock, this.callbackManager);
        }

        public delegate void ResultValidator(T result);

        public ResultValidator CallbackResultValidator { get; set; }

        public IDictionary<string, object> ExpectedResult { private get; set; }

        public AndroidFacebook AndroidFacebook { get; private set; }

        public void Run(Action<FacebookDelegate<T>> method, MockAndroidJavaClass.OnResult handler)
        {
            string expectedResultStr = MiniJSON.Json.Serialize(this.ExpectedResult);
            this.mock.Callback = handler;
            this.mock.Result = expectedResultStr;
            bool callbackCalled = false;

            FacebookDelegate<T> callback = delegate(T result)
            {
                if (this.CallbackResultValidator != null)
                {
                    this.CallbackResultValidator(result);
                }

                Assert.AreEqual(expectedResultStr, result.RawResult);
                callbackCalled = true;
            };

            method(callback);
            Assert.IsTrue(callbackCalled);
        }
    }
}
