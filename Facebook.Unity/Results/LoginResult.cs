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
    using System.Collections.Generic;

    internal class LoginResult : ResultBase, ILoginResult
    {
        public const string LastRefreshKey = "last_refresh";
        public static readonly string UserIdKey = Constants.IsWeb ? "userID" : "user_id";
        public static readonly string ExpirationTimestampKey = Constants.IsWeb ? "expiresIn" : "expiration_timestamp";
        public static readonly string PermissionsKey = Constants.IsWeb ? "grantedScopes" : "permissions";
        public static readonly string AccessTokenKey = Constants.IsWeb ? "accessToken" : Constants.AccessTokenKey;
        public static readonly string GraphDomain = "graph_domain";

        internal LoginResult(ResultContainer resultContainer) : base(resultContainer)
        {
            if (this.ResultDictionary != null && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
            {
                this.AccessToken = Utilities.ParseAccessTokenFromResult(this.ResultDictionary);
            }
        }

        public AccessToken AccessToken { get; private set; }

        public override string ToString()
        {
            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "AccessToken", this.AccessToken.ToStringNullOk() },
                });
        }
    }
}
