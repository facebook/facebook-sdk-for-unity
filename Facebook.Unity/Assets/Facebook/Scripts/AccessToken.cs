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
    using UnityEngine;

    /// <summary>
    /// Contains the access token and related information.
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook.AccessToken"/> class.
        /// </summary>
        /// <param name="tokenString">Token string.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="expirationTime">Expiration time.</param>
        /// <param name="permissions">Permissions.</param>
        public AccessToken(
            string tokenString,
            string userId,
            DateTime expirationTime,
            IEnumerable<string> permissions)
        {
            if (string.IsNullOrEmpty(tokenString))
            {
                throw new ArgumentNullException("tokenString");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            if (expirationTime == DateTime.MinValue)
            {
                throw new ArgumentException("Expiration time is unassigned");
            }

            if (permissions == null)
            {
                throw new ArgumentNullException("permissions");
            }

            this.TokenString = tokenString;
            this.ExpirationTime = expirationTime;
            this.Permissions = permissions;
            this.UserId = userId;
        }

        /// <summary>
        /// Gets or sets the current access token.
        /// </summary>
        /// <value>The current access token.</value>
        public static AccessToken CurrentAccessToken { get; internal set; }

        /// <summary>
        /// Gets or sets the token string.
        /// </summary>
        /// <value>The token string.</value>
        public string TokenString { get; internal set; }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        /// <value>The expiration time.</value>
        public DateTime ExpirationTime { get; internal set; }

        /// <summary>
        /// Gets or sets the list of permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public IEnumerable<string> Permissions { get; internal set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; internal set; }

        internal string ToJson()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary[LoginResult.PermissionsKey] = string.Join(",", this.Permissions.ToArray());
            dictionary[LoginResult.ExpirationTimestampKey] = this.ExpirationTime.TotalSeconds().ToString();
            dictionary[LoginResult.AccessTokenKey] = this.TokenString;
            dictionary[LoginResult.UserIdKey] = this.UserId;

            return MiniJSON.Json.Serialize(dictionary);
        }
    }
}
