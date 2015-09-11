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

namespace Facebook.Unity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class LoginResult : ResultBase, ILoginResult
    {
        public static readonly string UserIdKey = Constants.IsWeb ? "userID" : "user_id";
        public static readonly string ExpirationTimestampKey = Constants.IsWeb ? "expiresIn" : "expiration_timestamp";
        public static readonly string PermissionsKey = Constants.IsWeb ? "grantedScopes" : "permissions";
        public static readonly string AccessTokenKey = Constants.IsWeb ? "accessToken" : Constants.AccessTokenKey;

        internal LoginResult(string response) : base(response)
        {
            if (this.ResultDictionary != null && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
            {
                this.AccessToken = LoginResult.ParseAccessTokenFromResult(this.ResultDictionary);
            }
        }

        public AccessToken AccessToken { get; private set; }

        private static AccessToken ParseAccessTokenFromResult(IDictionary<string, object> resultDictionary)
        {
            string userID = resultDictionary.GetValueOrDefault<string>(LoginResult.UserIdKey);
            string accessToken = resultDictionary.GetValueOrDefault<string>(LoginResult.AccessTokenKey);
            DateTime expiration = LoginResult.ParseExpirationDateFromResult(resultDictionary);
            ICollection<string> permissions = LoginResult.ParsePermissionFromResult(resultDictionary);

            return new AccessToken(
                accessToken,
                userID,
                expiration,
                permissions);
        }

        private static DateTime ParseExpirationDateFromResult(IDictionary<string, object> resultDictionary)
        {
            DateTime expiration;
            if (Constants.IsWeb)
            {
                // For canvas we get back the time as seconds since now instead of in epoch time.
                expiration = DateTime.Now.AddSeconds(resultDictionary.GetValueOrDefault<long>(LoginResult.ExpirationTimestampKey));
            }
            else
            {
                string expirationStr = resultDictionary.GetValueOrDefault<string>(LoginResult.ExpirationTimestampKey);
                int expiredTimeSeconds;
                if (int.TryParse(expirationStr, out expiredTimeSeconds) && expiredTimeSeconds > 0)
                {
                    expiration = LoginResult.FromTimestamp(expiredTimeSeconds);
                }
                else
                {
                    expiration = DateTime.MaxValue;
                }
            }

            return expiration;
        }

        private static ICollection<string> ParsePermissionFromResult(IDictionary<string, object> resultDictionary)
        {
            string permissions;
            IEnumerable<object> permissionList;

            // For permissions we can get the result back in either a comma separated string or
            // a list depending on the platform.
            if (resultDictionary.TryGetValue(LoginResult.PermissionsKey, out permissions))
            {
                permissionList = permissions.Split(',');
            }
            else if (!resultDictionary.TryGetValue(LoginResult.PermissionsKey, out permissionList))
            {
                permissionList = new string[0];
                FacebookLogger.Warn("Failed to find parameter '{0}' in login result", LoginResult.PermissionsKey);
            }

            return permissionList.Select(permission => permission.ToString()).ToList();
        }

        private static DateTime FromTimestamp(int timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }

        private AccessToken ParseAccessTokeFromResponse()
        {
            if (this.ResultDictionary == null ||
                !this.ResultDictionary.ContainsKey("user_id"))
            {
                return null;
            }

            string userId = (string)this.ResultDictionary["user_id"];
            string accessToken = (string)this.ResultDictionary["access_token"];

            int expiredTimeSeconds;
            DateTime accessTokenExpiresAt;

            // If the date time is very large or 0 assume the token never expires
            if (int.TryParse((string)this.ResultDictionary["expiration_timestamp"], out expiredTimeSeconds) && expiredTimeSeconds > 0)
            {
                accessTokenExpiresAt = FromTimestamp(expiredTimeSeconds);
            }
            else
            {
                accessTokenExpiresAt = DateTime.MaxValue;
            }

            // Permissions can be array or string
            ICollection<string> permissions;
            string permissionStr = this.ResultDictionary["permissions"] as string;
            if (permissionStr != null)
            {
                permissions = permissionStr.Split(',');
            }
            else
            {
                // Assume we have an list
                var rawPermissions = (IEnumerable<object>)this.ResultDictionary["permissions"];
                permissions = rawPermissions.Select(permission => permission.ToString()).ToList();
            }

            return new AccessToken(
                accessToken,
                userId,
                accessTokenExpiresAt,
                permissions);
        }
    }
}
