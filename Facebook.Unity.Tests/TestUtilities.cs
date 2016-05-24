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

    public static class TestUtilities
    {
        public static long TimeInSeconds(this DateTime dateTime)
        {
            TimeSpan t = dateTime - new DateTime(1970, 1, 1);
            long secondsSinceEpoch = (long)t.TotalSeconds;
            return secondsSinceEpoch;
        }

        public static string ToJson(this IDictionary<string, object> dictionary)
        {
            return MiniJSON.Json.Serialize(dictionary);
        }

        public static void AddAllKVPFrom<T1, T2>(this IDictionary<T1, T2> dest, IDictionary<T1, T2> source)
        {
            foreach (T1 key in source.Keys)
            {
                dest[key] = source[key];
            }
        }

        /// <summary>
        /// Sets the value for the specified key if the dictionary does not already contain a value
        /// for this key.
        /// </summary>
        public static bool TrySetKey<T1, T2>(this IDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }

            dictionary[key] = value;
            return true;
        }
    }
}
