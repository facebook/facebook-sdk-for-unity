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

    public class FBLocation
    {
        internal FBLocation(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        /// <summary>
        /// Gets the location's unique identifier
        /// </summary>
        /// <value>location's unique identifier.</value>
        public string ID { get; private set; }

        /// <summary>
        /// Gets the location's name.
        /// </summary>
        /// <value>The location's name.</value>
        public string Name { get; private set; }


        internal static FBLocation FromDictionary(string prefix, IDictionary<string, string> dictionary)
        {
            string id;
            string name;
            dictionary.TryGetValue(prefix + "_id", out id);
            dictionary.TryGetValue(prefix + "_name", out name);
            if (id == null || name == null) {
                return null;
            }
            return new FBLocation(id, name);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.FBLocation"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.FBLocation"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "id", this.ID},
                    { "name", this.Name},
                });
        }
    }
}
