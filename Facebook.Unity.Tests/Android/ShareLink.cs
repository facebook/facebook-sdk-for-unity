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

    public class ShareLink : FacebookTestClass
    {
        [Test]
        public void SimpleLinkShare()
        {
            var test = new AndroidTest<IShareResult>();
            var result = new Dictionary<string, object>();
            result[Constants.CallbackIdKey] = "1";
            result["id"] = "123456789";
            test.ExpectedResult = result;
            test.CallbackResultValidator = (shareResult) =>
            {
                Assert.AreEqual(result["id"], shareResult.PostId);
            };

            test.Run(
                (callback) => test.AndroidFacebook.ShareLink(
                    new Uri("http://www.test.com/"),
                    "test title",
                    "test description",
                    new Uri("http://www.photo.com/"),
                    callback),
                test.AndroidFacebook.OnShareLinkComplete);
        }
    }
}
