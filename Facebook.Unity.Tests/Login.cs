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
    using NUnit.Framework;

    public abstract class Login : FacebookTestClass
    {
        protected readonly string[] ReadPermissions = new string[]
        {
            Constants.EmailPermission,
            Constants.UserLikesPermission
        };

        protected readonly string[] PublishPermissions = new string[]
        {
            Constants.PublishActionsPermission,
            Constants.PublishPagesPermission
        };

        [Test]
        public void BasicLoginWithReadTest()
        {
            ILoginResult result = null;
            FB.LogInWithReadPermissions(
                null,
                delegate(ILoginResult r)
                {
                    result = r;
                });
            Login.ValidateToken(result, MockResults.DefaultPermissions);
        }

        [Test]
        public void BasicLoginWithReadPermissionsTest()
        {
            ILoginResult result = null;
            FB.LogInWithReadPermissions(
                this.ReadPermissions,
                delegate(ILoginResult r)
                {
                    result = r;
                });
            Login.ValidateToken(result, this.ReadPermissions);
        }

        [Test]
        public void BasicLoginWithPublishTest()
        {
            ILoginResult result = null;
            FB.LogInWithPublishPermissions(
                this.PublishPermissions,
                delegate(ILoginResult r)
                {
                    result = r;
                });
            Login.ValidateToken(result, this.PublishPermissions);
        }

        [Test]
        public void IsLoggedInNullAccessToken()
        {
            AccessToken.CurrentAccessToken = null;
            Assert.IsFalse(FB.IsLoggedIn);
        }

        [Test]
        public void IsLoggedInValidExpiration()
        {
            AccessToken.CurrentAccessToken = new AccessToken(
                "faketokenstring",
                "1",
                DateTime.UtcNow.AddDays(1),
                new List<string>(), 
                null,
                "facebook");
            Assert.IsTrue(FB.IsLoggedIn);
        }

        [Test]
        public void IsLoggedInValidExpiredToken()
        {
            AccessToken.CurrentAccessToken = new AccessToken(
                "faketokenstring",
                "1",
                DateTime.UtcNow.AddDays(-1),
                new List<string>(), 
                null,
                "facebook");
            Assert.IsFalse(FB.IsLoggedIn);
        }

        protected override void OnInit()
        {
            base.OnInit();

            // Before each test clear the state of the access token
            AccessToken.CurrentAccessToken = null;
        }

        private static void ValidateToken(ILoginResult loginResult, IEnumerable<string> permissions)
        {
            Assert.IsNotNull(loginResult);
            AccessToken token = AccessToken.CurrentAccessToken;
            Assert.AreSame(loginResult.AccessToken, loginResult.AccessToken);
            Assert.IsNotNull(token);

            // For canvas we can be off by about a second since the token value for expiration time is
            // returned in the format seconds until expired from now.
            long diff = token.ExpirationTime.TotalSeconds() - MockResults.MockExpirationTimeValue.TotalSeconds();
            Assert.IsTrue(Math.Abs(diff) < 5);

            Assert.AreEqual(MockResults.MockTokenStringValue, token.TokenString);
            Assert.AreEqual(MockResults.MockUserIDValue, token.UserId);
            Assert.AreEqual(permissions.Count(), token.Permissions.Count());

            diff = token.LastRefresh.Value.TotalSeconds() - MockResults.MockLastRefresh.TotalSeconds();
            Assert.IsTrue(Math.Abs(diff) < 5);
            foreach (var perm in permissions)
            {
                Assert.IsTrue(token.Permissions.Contains(perm));
            }
        }
    }
}
