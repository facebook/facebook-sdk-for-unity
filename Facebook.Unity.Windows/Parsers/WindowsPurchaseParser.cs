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

using System.Collections;
using System.Collections.Generic;

namespace Facebook.Unity.Windows
{
    class WindowsPurchaseParser : WindowsParserBase
    {
        public static ResultContainer Parse(fbg.Purchases purchases, string callbackId, bool onlyFirst = false)
        {
            IDictionary<string, object> deserializedPurchaseData = Facebook.MiniJSON.Json.Deserialize(purchases.Raw) as Dictionary<string, object>;
            ResultContainer container;
            if (deserializedPurchaseData.TryGetValue("data", out IList apiData))
            {
                IList<Purchase> purchasesList = new List<Purchase>();

                foreach (IDictionary<string, object> purchase in apiData)
                {
                    purchasesList.Add(Utilities.ParsePurchaseFromDictionary(purchase, true));
                }

                Dictionary<string, object> result = new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", purchases.Raw}
                };

                if (purchasesList.Count >= 1)
                {
                    if (onlyFirst)
                    {
                        result["purchase"] = purchasesList[0];
                    }
                    else
                    {
                        result["purchases"] = purchasesList;
                    }
                }
                else
                {
                    result[Constants.ErrorKey] = "ERROR: Parsing purchases. No purchase data.";
                }

                container = new ResultContainer(result);
            }
            else
            {
                container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", purchases.Raw},
                    {Constants.ErrorKey, "ERROR: Parsing purchases. Wrong data format"}
                });
            }
            return container;
        }
    }
}
