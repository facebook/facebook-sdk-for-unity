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
    using NUnit.Framework;

    public abstract class AppRequest : FacebookTestClass
    {
        [Test]
        public void TestAppRequest()
        {
            IAppRequestResult result = null;

            string mockRequestId = "1234567890";
            var toList = new List<string>() { "1234567890", "9999999999" };

            this.Mock.ResultExtras = new Dictionary<string, object>()
            {
                { AppRequestResult.RequestIDKey, mockRequestId },
                { AppRequestResult.ToKey, string.Join(",", toList) },
            };

            FB.AppRequest(
                "Test message",
                callback:
                    delegate(IAppRequestResult r)
                    {
                        result = r;
                    });
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RequestID, mockRequestId);
            Assert.IsTrue(new HashSet<string>(toList).SetEquals(result.To));
        }
    }
}
