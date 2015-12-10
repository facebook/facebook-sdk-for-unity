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

    internal class MethodArguments
    {
        private IDictionary<string, object> arguments = new Dictionary<string, object>();

        public MethodArguments() : this(new Dictionary<string, object>())
        {
        }

        public MethodArguments(MethodArguments methodArgs) : this(methodArgs.arguments)
        {
        }

        private MethodArguments(IDictionary<string, object> arguments)
        {
            this.arguments = arguments;
        }

        public void AddPrimative<T>(string argumentName, T value) where T : struct
        {
            this.arguments[argumentName] = value;
        }

        public void AddNullablePrimitive<T>(string argumentName, T? nullable) where T : struct
        {
            if (nullable != null && nullable.HasValue)
            {
                this.arguments[argumentName] = nullable.Value;
            }
        }

        public void AddString(string argumentName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.arguments[argumentName] = value;
            }
        }

        public void AddCommaSeparatedList(string argumentName, IEnumerable<string> value)
        {
            if (value != null)
            {
                this.arguments[argumentName] = value.ToCommaSeparateList();
            }
        }

        public void AddDictionary(string argumentName, IDictionary<string, object> dict)
        {
            if (dict != null)
            {
                this.arguments[argumentName] = MethodArguments.ToStringDict(dict);
            }
        }

        public void AddList<T>(string argumentName, IEnumerable<T> list)
        {
            if (list != null)
            {
                this.arguments[argumentName] = list;
            }
        }

        public void AddUri(string argumentName, Uri uri)
        {
            if (uri != null && !string.IsNullOrEmpty(uri.AbsoluteUri))
            {
                this.arguments[argumentName] = uri.ToString();
            }
        }

        public string ToJsonString()
        {
            return MiniJSON.Json.Serialize(this.arguments);
        }

        private static Dictionary<string, string> ToStringDict(IDictionary<string, object> dict)
        {
            if (dict == null)
            {
                return null;
            }

            var newDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                newDict[kvp.Key] = kvp.Value.ToString();
            }

            return newDict;
        }
    }
}
