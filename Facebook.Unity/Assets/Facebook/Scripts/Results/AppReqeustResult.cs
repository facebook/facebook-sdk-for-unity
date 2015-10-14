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
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal class AppRequestResult : ResultBase, IAppRequestResult
    {
        public AppRequestResult(string result) : base(result)
        {
            if (this.ResultDictionary != null)
            {
                string requestID;
                if (this.ResultDictionary.TryGetValue<string>("request", out requestID))
                {
                    this.RequestID = requestID;
                }

                string toStr;
                if (this.ResultDictionary.TryGetValue<string>("to", out toStr))
                {
                    this.To = toStr.Split(',');
                }
                else
                {
                    // On iOS the to field is an array of IDs
                    IEnumerable<object> toArray;
                    if (this.ResultDictionary.TryGetValue("to", out toArray))
                    {
                        var toList = new List<string>();
                        foreach (object toEntry in toArray)
                        {
                            var toID = toEntry as string;
                            if (toID != null)
                            {
                                toList.Add(toID);
                            }
                        }

                        this.To = toList;
                    }
                }
            }
        }

        public string RequestID { get; private set; }

        public IEnumerable<string> To { get; private set; }
    }
}
