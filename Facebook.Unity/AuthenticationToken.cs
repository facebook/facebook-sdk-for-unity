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
    using System.Collections.Generic;

    public class AuthenticationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationToken"/> class.
        /// </summary>
        /// <param name="tokenString">Token string of the token.</param>
        /// <param name="nonce">Nonce of the token.</param>
        internal AuthenticationToken(
            string tokenString,
            string nonce)
        {
            if (string.IsNullOrEmpty(tokenString))
            {
                throw new ArgumentNullException("AuthTokenString");
            }

            if (string.IsNullOrEmpty(nonce))
            {
                throw new ArgumentNullException("AuthNonce");
            }

            this.TokenString = tokenString;
            this.Nonce = nonce;
        }

        /// <summary>
        /// Gets the token string.
        /// </summary>
        /// <value>The token string.</value>
        public string TokenString { get; private set; }

        /// <summary>
        /// Gets the nonce string.
        /// </summary>
        /// <value>The nonce string.</value>
        public string Nonce { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.AuthenticationToken"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.AuthenticationToken"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "TokenString", this.TokenString},
                    { "Nonce", this.Nonce },
                });
        }

        internal string ToJson()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary[LoginResult.AuthTokenString] = this.TokenString;
            dictionary[LoginResult.AuthNonce] = this.Nonce;

            return MiniJSON.Json.Serialize(dictionary);
        }
    }
}
