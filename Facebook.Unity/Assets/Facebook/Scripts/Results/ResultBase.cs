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

    internal abstract class ResultBase : IInternalResult
    {
        internal ResultBase(string result)
        {
            string error = null;
            bool cancelled = false;
            string callbackId = null;
            if (!string.IsNullOrEmpty(result))
            {
                var dictionary = Facebook.MiniJSON.Json.Deserialize(result) as Dictionary<string, object>;
                if (dictionary != null)
                {
                    this.ResultDictionary = dictionary;
                    error = ResultBase.GetErrorValue(dictionary);
                    cancelled = ResultBase.GetCancelledValue(dictionary);
                    callbackId = ResultBase.GetCallbackId(dictionary);
                }
            }

            this.Init(result, error, cancelled, callbackId);
        }

        internal ResultBase(string result, string error, bool cancelled)
        {
            this.Init(result, error, cancelled, null);
        }

        public virtual string Error { get; protected set; }

        public virtual IDictionary<string, object> ResultDictionary { get; protected set; }

        public virtual string RawResult { get; protected set; }

        public virtual bool Cancelled { get; protected set; }

        public virtual string CallbackId { get; protected set; }

        public override string ToString()
        {
            return string.Format(
                "[BaseResult: Error={0}, Result={1}, RawResult={2}, Cancelled={3}]",
                this.Error,
                this.ResultDictionary,
                this.RawResult,
                this.Cancelled);
        }

        protected void Init(string result, string error, bool cancelled, string callbackId)
        {
            this.RawResult = result;
            this.Cancelled = cancelled;
            this.Error = error;
            this.CallbackId = callbackId;
        }

        private static string GetErrorValue(IDictionary<string, object> result)
        {
            if (result == null)
            {
                return null;
            }

            string error;
            if (result.TryGetValue<string>("error", out error))
            {
                return error;
            }

            return null;
        }

        private static bool GetCancelledValue(IDictionary<string, object> result)
        {
            if (result == null)
            {
                return false;
            }

            // Check for cancel string
            object cancelled;
            if (result.TryGetValue("cancelled", out cancelled))
            {
                bool? cancelBool = cancelled as bool?;
                if (cancelBool != null)
                {
                    return cancelBool.HasValue && cancelBool.Value;
                }

                string cancelString = cancelled as string;
                if (cancelString != null)
                {
                    return Convert.ToBoolean(cancelString);
                }

                int? cancelInt = cancelled as int?;
                if (cancelInt != null)
                {
                    return cancelInt.HasValue && cancelInt.Value != 0;
                }
            }

            return false;
        }

        private static string GetCallbackId(IDictionary<string, object> result)
        {
            if (result == null)
            {
                return null;
            }

            // Check for cancel string
            string callbackId;
            if (result.TryGetValue<string>(Constants.CallbackIdKey, out callbackId))
            {
                return callbackId;
            }

            return null;
        }
    }
}
