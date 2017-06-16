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
    using System.Linq;
    using NUnit.Framework;

    public abstract class Init : FacebookTestClass
    {
        [Test]
        public void BasicInit()
        {
            bool initComplete = false;
            this.CallInit(
                delegate
                {
                    initComplete = true;
                });
            Assert.IsTrue(initComplete);
        }

        [Test]
        public void InitWithLoginData()
        {
            this.Mock.ResultExtras = MockResults.GetLoginResult(1, "email", null);
            AccessToken.CurrentAccessToken = null;
            bool initComplete = false;

            this.CallInit(
                delegate
                {
                    initComplete = true;
                });
            Assert.IsTrue(initComplete);

            AccessToken token = AccessToken.CurrentAccessToken;
            Assert.IsNotNull(token);

            long diff = token.ExpirationTime.TotalSeconds() - MockResults.MockExpirationTimeValue.TotalSeconds();
            Assert.IsTrue(Math.Abs(diff) < 5);

            Assert.AreEqual(MockResults.MockTokenStringValue, token.TokenString);
            Assert.AreEqual(MockResults.MockUserIDValue, token.UserId);
            Assert.AreEqual(1, token.Permissions.Count());
        }

        protected abstract void CallInit(InitDelegate callback);
    }
}
