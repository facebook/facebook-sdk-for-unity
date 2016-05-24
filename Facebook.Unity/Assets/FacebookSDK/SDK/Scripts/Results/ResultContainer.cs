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

    internal class ResultContainer
    {
        private const string CanvasResponseKey = "response";

        public ResultContainer(IDictionary<string, object> dictionary)
        {
            this.RawResult = dictionary.ToJson();
            this.ResultDictionary = dictionary;
            if (Constants.IsWeb)
            {
                this.ResultDictionary = this.GetWebFormattedResponseDictionary(this.ResultDictionary);
            }
        }

        public ResultContainer(string result)
        {
            this.RawResult = result;

            if (!string.IsNullOrEmpty(result))
            {
                this.ResultDictionary = Facebook.MiniJSON.Json.Deserialize(result) as Dictionary<string, object>;

                if (Constants.IsWeb)
                {
                    // Web has a different format from mobile so reformat the result to match our
                    // mobile responses
                    this.ResultDictionary = this.GetWebFormattedResponseDictionary(this.ResultDictionary);
                }
            }
        }

        public string RawResult { get; private set; }

        public IDictionary<string, object> ResultDictionary { get; set; }

        private IDictionary<string, object> GetWebFormattedResponseDictionary(IDictionary<string, object> resultDictionary)
        {
            IDictionary<string, object> responseDictionary;
            if (resultDictionary.TryGetValue(CanvasResponseKey, out responseDictionary))
            {
                object callbackId;
                if (resultDictionary.TryGetValue(Constants.CallbackIdKey, out callbackId))
                {
                    responseDictionary[Constants.CallbackIdKey] = callbackId;
                }

                return responseDictionary;
            }

            return resultDictionary;
        }
    }
}
