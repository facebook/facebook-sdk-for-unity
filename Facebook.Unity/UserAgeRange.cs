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

    public class UserAgeRange
    {
        internal UserAgeRange(long min, long max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Gets the user's minimun age, -1 if unspecified.
        /// </summary>
        /// <value>The user's minimun age</value>
        public long Min { get; private set; }

        /// <summary>
        /// Gets the user's maximun age, -1 if unspecified.
        /// </summary>
        /// <value>The user's maximun age.</value>
        public long Max { get; private set; }


        internal static UserAgeRange AgeRangeFromDictionary(IDictionary<string, string> dictionary)
        {
            string minStr;
            string maxStr;
            long min;
            long max;
            dictionary.TryGetValue("ageMin", out minStr);
            dictionary.TryGetValue("ageMax", out maxStr);
            min = long.TryParse(minStr, out min) ? min : -1;
            max = long.TryParse(maxStr, out max) ? max : -1;
            if (min < 0 && max < 0)
            {
                return null;
            }
            return new UserAgeRange(min, max);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.UserAgeRange"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.UserAgeRange"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "Min", this.Min.ToString()},
                    { "Max", this.Max.ToString()},
                });
        }
    }
}
