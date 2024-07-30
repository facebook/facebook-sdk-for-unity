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
    class WindowsCatalogParser : WindowsParserBase
    {
        public static ResultContainer Parse(fbg.Catalog catalog, string callbackId)
        {
            IDictionary<string, object> deserializedCatalogData = Facebook.MiniJSON.Json.Deserialize(catalog.Raw) as Dictionary<string, object>;

            ResultContainer container;
            if (deserializedCatalogData.TryGetValue("data", out IList apiData))
            {
                IList<Product> products = new List<Product>();
                foreach (IDictionary<string, object> product in apiData)
                {
                    products.Add(Utilities.ParseProductFromCatalogResult(product, true));
                }

                container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", catalog.Raw},
                    {"products", products}
                });
            }
            else
            {
                container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", catalog.Raw},
                    {Constants.ErrorKey, "ERROR: Parsing catalog. Wrong data format"}
                });
            }
            return container;
        }
    }
}
