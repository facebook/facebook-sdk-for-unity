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

namespace Facebook.Unity.Tests.Mobile
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public abstract class AppLinks : Facebook.Unity.Tests.AppLinks
    {
        [Test]
        public void TestEmptyDefferedAppLink()
        {
            IAppLinkResult result = null;

            string mockRef = "mockref";
            string mockTargetUrl = "mocktargeturl";
            var mockExtras = new Dictionary<string, object>()
            {
                { "com.facebook.platform.APPLINK_NATIVE_CLASS", string.Empty },
            };

            this.Mock.ResultExtras = new Dictionary<string, object>()
            {
                { Constants.RefKey, mockRef },
                { Constants.TargetUrlKey, mockTargetUrl },
                { Constants.ExtrasKey, mockExtras },
            };

            FB.GetAppLink(
                delegate(IAppLinkResult r)
                {
                    result = r;
                });
            Assert.IsNotNull(result);
            Assert.AreEqual(mockRef, result.Ref);
            Assert.AreEqual(mockTargetUrl, result.TargetUrl);
            Assert.AreEqual(mockExtras.ToJson(), result.Extras.ToJson());
        }

        [Test]
        public void TestSimpleDeepLink()
        {
            IAppLinkResult result = null;

            string mockUrl = "mockurl";
            string mockRef = "mockref";
            string mockTargetUrl = "mocktargeturl";

            this.Mock.ResultExtras = new Dictionary<string, object>()
            {
                { Constants.RefKey, mockRef },
                { Constants.TargetUrlKey, mockTargetUrl },
                { Constants.UrlKey, mockUrl },
            };

            FB.GetAppLink(
                delegate(IAppLinkResult r)
                {
                    result = r;
                });
            Assert.IsNotNull(result);
            Assert.AreEqual(mockRef, result.Ref);
            Assert.AreEqual(mockTargetUrl, result.TargetUrl);
            Assert.AreEqual(mockUrl, result.Url);
        }

        [Test]
        public void TestAppLink()
        {
            IAppLinkResult result = null;

            string mockUrl = "mockurl";

            this.Mock.ResultExtras = new Dictionary<string, object>()
            {
                { Constants.UrlKey, mockUrl },
            };

            FB.GetAppLink(
                delegate(IAppLinkResult r)
                {
                    result = r;
                });
            Assert.IsNotNull(result);
            Assert.AreEqual(mockUrl, result.Url);
        }
    }
}
