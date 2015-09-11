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

    public class AppLinks : FacebookTestClass
    {
        [Test]
        public void TestAndroidEmptyDefferedAppLink()
        {
            var test = new AndroidTest<IAppLinkResult>();
            var result = new Dictionary<string, object>();
            result[Constants.CallbackIdKey] = "1";
            result["did_complete"] = true;
            test.ExpectedResult = result;

            test.Run(
                (callback) => test.AndroidFacebook.FetchDeferredAppLink(callback),
                test.AndroidFacebook.OnFetchDeferredAppLinkComplete);
        }

        [Test]
        public void TestAndroidDeferredAppLink()
        {
            var test = new AndroidTest<IAppLinkResult>();
            var result = new Dictionary<string, object>();
            result[Constants.CallbackIdKey] = "1";
            result[Constants.RefKey] = "test ref";
            result[Constants.TargetUrlKey] = "test target url";
            result[Constants.ExtrasKey] = new Dictionary<string, object>()
            {
                {
                    "com.facebook.platform.APPLINK_NATIVE_CLASS", string.Empty
                },
                {
                    "target_url", result[Constants.TargetUrlKey]
                }
            };

            test.ExpectedResult = result;

            test.CallbackResultValidator = appLinkResult =>
            {
                Assert.IsNotNull(appLinkResult.Extras);
                Assert.IsNotNull(appLinkResult.Ref);
                Assert.IsNotNull(appLinkResult.TargetUrl);
                Assert.IsNull(appLinkResult.Url);
            };

            test.Run(
                (callback) => test.AndroidFacebook.FetchDeferredAppLink(callback),
                test.AndroidFacebook.OnFetchDeferredAppLinkComplete);
        }

        [Test]
        public void TestAndroidSimpleDeepLink()
        {
            var test = new AndroidTest<IAppLinkResult>();
            var result = new Dictionary<string, object>();
            result[Constants.CallbackIdKey] = "1";
            result[Constants.UrlKey] = "test url";
            result[Constants.RefKey] = "test ref";
            result[Constants.TargetUrlKey] = "test target url";
            result[Constants.ExtrasKey] = new Dictionary<string, object>()
            {
                {
                    "extras", new Dictionary<string, object>()
                },
                {
                    "version", "1.0"
                }
            };

            test.ExpectedResult = result;

            test.CallbackResultValidator = appLinkResult =>
            {
                Assert.AreEqual(result[Constants.UrlKey], appLinkResult.Url);
                Assert.AreEqual(result[Constants.RefKey], appLinkResult.Ref);
                Assert.AreEqual(result[Constants.TargetUrlKey], appLinkResult.TargetUrl);
                Assert.AreEqual(
                    MiniJSON.Json.Serialize(result[Constants.ExtrasKey]),
                    MiniJSON.Json.Serialize(appLinkResult.Extras));
            };

            test.Run(
                (callback) => test.AndroidFacebook.GetAppLink(callback),
                test.AndroidFacebook.OnGetAppLinkComplete);
        }

        [Test]
        public void TestAndroidAppLink()
        {
            var test = new AndroidTest<IAppLinkResult>();
            var result = new Dictionary<string, object>();
            result[Constants.CallbackIdKey] = "1";
            result[Constants.UrlKey] = "test url";

            test.ExpectedResult = result;

            test.CallbackResultValidator = appLinkResult =>
            {
                Assert.AreEqual(result[Constants.UrlKey], appLinkResult.Url);
                Assert.IsNull(appLinkResult.Ref);
                Assert.IsNull(appLinkResult.TargetUrl);
                Assert.IsNull(appLinkResult.Extras);
            };

            test.Run(
                (callback) => test.AndroidFacebook.GetAppLink(callback),
                test.AndroidFacebook.OnGetAppLinkComplete);
        }
    }
}
