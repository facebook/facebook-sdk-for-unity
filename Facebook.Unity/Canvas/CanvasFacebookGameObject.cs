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

namespace Facebook.Unity.Canvas
{
    using UnityEngine;

    internal class CanvasFacebookGameObject : FacebookGameObject, ICanvasFacebookCallbackHandler
    {
        protected ICanvasFacebookImplementation CanvasFacebookImpl
        {
            get
            {
                return (ICanvasFacebookImplementation)this.Facebook;
            }
        }

        public void OnPayComplete(string result)
        {
            this.CanvasFacebookImpl.OnPayComplete(new ResultContainer(result));
        }

        public void OnFacebookAuthResponseChange(string message)
        {
            this.CanvasFacebookImpl.OnFacebookAuthResponseChange(new ResultContainer(message));
        }

        public void OnUrlResponse(string message)
        {
            this.CanvasFacebookImpl.OnUrlResponse(message);
        }

        public void OnHideUnity(bool hide)
        {
            this.CanvasFacebookImpl.OnHideUnity(hide);
        }

        protected override void OnAwake()
        {
            // Facebook JS Bridge lives in it's own gameobject for optimization reasons
            // see UnityObject.SendMessage()
            var bridgeObject = new GameObject("FacebookJsBridge");
            bridgeObject.AddComponent<JsBridge>();
            bridgeObject.transform.parent = gameObject.transform;
        }
    }
}
