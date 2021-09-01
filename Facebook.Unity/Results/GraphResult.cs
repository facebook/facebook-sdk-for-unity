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

#pragma warning disable 618
namespace Facebook.Unity
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;

    internal class GraphResult : ResultBase, IGraphResult
    {
        internal GraphResult(UnityWebRequestAsyncOperation result) :
            base(new ResultContainer(result.webRequest.downloadHandler.text), result.webRequest.error, false)
        {
            this.Init(this.RawResult);

            // The WWW object will throw an exception if accessing the texture field and
            // an error has occured.
            if (string.IsNullOrEmpty(result.webRequest.error))
            {
                // The Graph API does not return textures directly, but a few endpoints can
                // redirect to images when no 'redirect=false' parameter is specified. Ex: '/me/picture'
                this.Texture = new Texture2D(2, 2);
                this.Texture.LoadImage(result.webRequest.downloadHandler.data);
            }
        }

        public IList<object> ResultList { get; private set; }

        public Texture2D Texture { get; private set; }

        private void Init(string rawResult)
        {
            if (string.IsNullOrEmpty(rawResult))
            {
                return;
            }

            object serailizedResult = MiniJSON.Json.Deserialize(this.RawResult);
            var jsonObject = serailizedResult as IDictionary<string, object>;
            if (jsonObject != null)
            {
                this.ResultDictionary = jsonObject;
                return;
            }

            var jsonArray = serailizedResult as IList<object>;
            if (jsonArray != null)
            {
                this.ResultList = jsonArray;
                return;
            }
        }
    }
}
#pragma warning restore 618
