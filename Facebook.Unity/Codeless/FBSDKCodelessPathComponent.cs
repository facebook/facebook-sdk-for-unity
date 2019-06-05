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

using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
    public class FBSDKCodelessPathComponent
    {
        public FBSDKCodelessPathComponent (Dictionary<string, object> dict)
        {
            className = (string) dict[Constants.EventBindingKeysClassName];
            if (className != null) {
                this.className = className;
            }

            if (dict.ContainsKey(Constants.EventBindingKeysText)) {
                text = (string) dict[Constants.EventBindingKeysText];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysHint)) {
                this.hint = (string) dict[Constants.EventBindingKeysHint];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysDescription)) {
                desc = (string) dict[Constants.EventBindingKeysDescription];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysIndex)) {
                this.index = (long) dict[Constants.EventBindingKeysIndex];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysTag)) {
                this.tag = (string) dict[Constants.EventBindingKeysTag];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysSection)) {
                section  = (long) dict[Constants.EventBindingKeysSection];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysRow)) {
                row = (long) dict[Constants.EventBindingKeysRow];
            }

            if (dict.ContainsKey(Constants.EventBindingKeysMatchBitmask)) {
                matchBitmask = (long) dict[Constants.EventBindingKeysMatchBitmask];
            }

        }

        public string className {get; set;}
        public string text {get; set;}
        public string hint {get; set;}
        public string desc {get; set;}
        public string tag {get; set;}
        public long index {get; set;}
        public long section {get; set;}
        public long row {get; set;}
        public long matchBitmask {get; set;}
    }
}
