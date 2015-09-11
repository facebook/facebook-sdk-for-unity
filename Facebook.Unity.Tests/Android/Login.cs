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
    using System.Linq;
    using Facebook.Unity.Mobile.Android;
    using NUnit.Framework;
    using NUnit.Mocks;

    [TestFixture]
    public class Login : FacebookTestClass
    {
        private const string UserID = "100000000000000";
        private const string TestTokenString = "This is a test token string";
        private readonly DateTime expirationTime = DateTime.UtcNow.AddDays(60);
        private readonly string[] readPermissions = new string[]
        {
            Constants.EmailPermission,
            Constants.UserLikesPermission
        };

        private readonly string[] publishPermissions = new string[]
        {
            Constants.PublishActionsPermission,
            Constants.PublishPagesPermission
        };

        [Test]
        public void BasicLoginWithReadTest()
        {
            AccessToken.CurrentAccessToken = null;
            var test = new AndroidTest<ILoginResult>();
            var result = this.GetBaseTokenResult();
            test.ExpectedResult = result;

            test.Run(
                (callback) => test.AndroidFacebook.LogInWithReadPermissions(null, callback),
                test.AndroidFacebook.OnLoginComplete);
            this.AssertResultDicMatchesToken(result, AccessToken.CurrentAccessToken);
        }

        [Test]
        public void BasicLoginWithPublishTest()
        {
            AccessToken.CurrentAccessToken = null;
            var test = new AndroidTest<ILoginResult>();
            var result = this.GetBaseTokenResult();
            result[LoginResult.PermissionsKey] = string.Join(",", this.publishPermissions);
            test.ExpectedResult = result;

            test.Run(
                (callback) => test.AndroidFacebook.LogInWithReadPermissions(null, callback),
                test.AndroidFacebook.OnLoginComplete);
            this.AssertResultDicMatchesToken(result, AccessToken.CurrentAccessToken);
        }

        [Test]
        public void TestMaxInt64ExpiredTime()
        {
            AccessToken.CurrentAccessToken = null;
            var test = new AndroidTest<ILoginResult>();
            var result = this.GetBaseTokenResult();
            result[LoginResult.ExpirationTimestampKey] = "9223372036854775";
            test.ExpectedResult = result;

            test.Run(
                (callback) => test.AndroidFacebook.LogInWithReadPermissions(null, callback),
                test.AndroidFacebook.OnLoginComplete);

            // Check to make sure the access token is set
            var expected = new Dictionary<string, object>(result);
            expected[LoginResult.ExpirationTimestampKey] = DateTime.MaxValue.TimeInSeconds().ToString();
            this.AssertResultDicMatchesToken(expected, AccessToken.CurrentAccessToken);
        }

        [Test]
        public void TestZeroExpiredTime()
        {
            AccessToken.CurrentAccessToken = null;
            var test = new AndroidTest<ILoginResult>();
            var result = this.GetBaseTokenResult();
            result[LoginResult.ExpirationTimestampKey] = "0";
            test.ExpectedResult = result;

            test.Run(
                (callback) => test.AndroidFacebook.LogInWithReadPermissions(null, callback),
                test.AndroidFacebook.OnLoginComplete);

            // Check to make sure the access token is set
            var expected = new Dictionary<string, object>(result);
            expected[LoginResult.ExpirationTimestampKey] = DateTime.MaxValue.TimeInSeconds().ToString();
            this.AssertResultDicMatchesToken(expected, AccessToken.CurrentAccessToken);
        }

        private IDictionary<string, object> GetBaseTokenResult()
        {
            var result = new Dictionary<string, object>();
            result[Constants.CallbackIdKey] = "1";
            result[LoginResult.UserIdKey] = UserID;
            result[LoginResult.ExpirationTimestampKey] = this.expirationTime.TimeInSeconds().ToString();
            result[LoginResult.AccessTokenKey] = TestTokenString;
            result[LoginResult.PermissionsKey] = string.Join(",", this.readPermissions);
            return result;
        }

        private void AssertResultDicMatchesToken(IDictionary<string, object> result, AccessToken token)
        {
            Assert.AreEqual(result[LoginResult.UserIdKey], AccessToken.CurrentAccessToken.UserId);
            Assert.AreEqual(
                result[LoginResult.ExpirationTimestampKey],
                AccessToken.CurrentAccessToken.ExpirationTime.TimeInSeconds().ToString());
            string permString = (string)result[LoginResult.PermissionsKey];
            foreach (var perm in permString.Split(','))
            {
                Assert.IsTrue(AccessToken.CurrentAccessToken.Permissions.ToList().Contains(perm));
            }

            Assert.AreEqual(result[Constants.AccessTokenKey], AccessToken.CurrentAccessToken.TokenString);
        }
    }
}
