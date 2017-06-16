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

    internal static class MockResults
    {
        private const string MockTokenString = "This is a test token string";
        private const string MockUserID = "100000000000000";
        private const string MockGroupID = "123456789";

        private static DateTime mockLastRefresh = DateTime.UtcNow;
        private static DateTime mockExpirationTime = DateTime.UtcNow.AddDays(60);

        public static DateTime MockExpirationTimeValue
        {
            get
            {
                return MockResults.mockExpirationTime;
            }
        }

        public static string MockTokenStringValue
        {
            get
            {
                return MockResults.MockTokenString;
            }
        }

        public static string MockUserIDValue
        {
            get
            {
                return MockResults.MockUserID;
            }
        }

        public static IEnumerable<string> DefaultPermissions
        {
            get
            {
                return new string[] { "email", "public_profile" };
            }
        }

        public static string MockGroupIDValue
        {
            get
            {
                return MockResults.MockGroupID;
            }
        }

        public static DateTime MockLastRefresh
        {
            get
            {
                return MockResults.mockLastRefresh;
            }
        }

        public static IDictionary<string, object> GetLoginResult(
            int requestID,
            string permissions,
            IDictionary<string, object> extras)
        {
            return MockResults.GetLoginResult(
                requestID,
                string.IsNullOrEmpty(permissions) ? null : permissions.Split(','),
                extras);
        }

        public static IDictionary<string, object> GetLoginResult(
            int requestID,
            IEnumerable<string> permissions,
            IDictionary<string, object> extras)
        {
            if (permissions == null || !permissions.Any())
            {
                permissions = MockResults.DefaultPermissions;
            }

            var result = MockResults.GetGenericResult(requestID, extras);

            object expirationTime;
            if (Constants.IsGameroom)
            {
                expirationTime = Math.Round((MockResults.MockExpirationTimeValue - DateTime.UtcNow).TotalSeconds).ToString();
            }
            else if (Constants.IsWeb)
            {
                expirationTime = (long)(MockResults.MockExpirationTimeValue - DateTime.UtcNow).TotalSeconds;
            }
            else
            {
                expirationTime = MockResults.MockExpirationTimeValue.TotalSeconds().ToString();
            }

            result.TrySetKey(LoginResult.ExpirationTimestampKey, expirationTime);
            result.TrySetKey(LoginResult.UserIdKey, MockResults.MockUserIDValue);
            result.TrySetKey(LoginResult.AccessTokenKey, MockResults.MockTokenStringValue);
            result.TrySetKey(LoginResult.PermissionsKey, string.Join(",", permissions));
            result.TrySetKey(LoginResult.LastRefreshKey, MockResults.mockLastRefresh.TotalSeconds().ToString());
            return result;
        }

        public static IDictionary<string, object> GetGroupCreateResult(int requestID, IDictionary<string, object> extras)
        {
            var result = MockResults.GetGenericResult(requestID, extras);
            result.TrySetKey(GroupCreateResult.IDKey, MockResults.MockGroupIDValue);
            return result;
        }

        public static IDictionary<string, object> GetGenericResult(int requestID, IDictionary<string, object> extras)
        {
            return MockResults.GetResultDictionary(requestID, extras);
        }

        public static IDictionary<string, object> GetResultDictionary(int requestID, IDictionary<string, object> extras)
        {
            var result = new Dictionary<string, object>()
            {
                { Constants.CallbackIdKey, requestID.ToString() }
            };

            if (extras != null)
            {
                result.AddAllKVPFrom(extras);
            }

            return result;
        }
    }
}
