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
    public class FBSDKEventBinding
    {
        public FBSDKEventBinding (Dictionary<string, object> dict)
        {
            eventName = (string) dict[Constants.EventBindingKeysEventName];
            if (eventName != null) {
                this.eventName = eventName;
            }

            eventType = (string) dict[Constants.EventBindingKeysEventType];
            if (eventType != null) {
                this.eventType = eventType;
            }

            appVersion = (string) dict[Constants.EventBindingKeysAppVersion];
            if (appVersion != null) {
                this.appVersion = appVersion;
            }

            eventName = (string) dict[Constants.EventBindingKeysEventName];
            if (eventName != null) {
                this.eventName = eventName;
            }

            var _path = (List<System.Object>) dict[Constants.EventBindingKeysPath];
            path = new List<FBSDKCodelessPathComponent> ();
            foreach(Dictionary<string, System.Object> p in _path) {
                var pathComponent = new FBSDKCodelessPathComponent(p);
                path.Add(pathComponent);
            }
        }

        public string eventName {get; set;}
        public string eventType {get; set;}
        public string appVersion {get; set;}
        public string pathType {get; set;}
        public List<FBSDKCodelessPathComponent> path {get; set;}
        public List<string> parameters {get; set;}
    }
}
