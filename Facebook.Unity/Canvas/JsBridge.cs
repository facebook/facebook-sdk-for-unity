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

    internal class JsBridge : MonoBehaviour
    {
        private ICanvasFacebookCallbackHandler facebook;

        public void Start()
        {
            this.facebook = ComponentFactory.GetComponent<CanvasFacebookGameObject>(
                ComponentFactory.IfNotExist.ReturnNull);
        }

        public void OnLoginComplete(string responseJsonData = "")
        {
            this.facebook.OnLoginComplete(responseJsonData);
        }

        public void OnFacebookAuthResponseChange(string responseJsonData = "")
        {
            this.facebook.OnFacebookAuthResponseChange(responseJsonData);
        }

        public void OnPayComplete(string responseJsonData = "")
        {
            this.facebook.OnPayComplete(responseJsonData);
        }

        public void OnAppRequestsComplete(string responseJsonData = "")
        {
            this.facebook.OnAppRequestsComplete(responseJsonData);
        }

        public void OnShareLinkComplete(string responseJsonData = "")
        {
            this.facebook.OnShareLinkComplete(responseJsonData);
        }

        public void OnGroupCreateComplete(string responseJsonData = "")
        {
            this.facebook.OnGroupCreateComplete(responseJsonData);
        }

        public void OnJoinGroupComplete(string responseJsonData = "")
        {
            this.facebook.OnGroupJoinComplete(responseJsonData);
        }

        public void OnFacebookFocus(string state)
        {
            this.facebook.OnHideUnity(state != "hide");
        }

        public void OnInitComplete(string responseJsonData = "")
        {
            this.facebook.OnInitComplete(responseJsonData);
        }

        public void OnUrlResponse(string url = "")
        {
            this.facebook.OnUrlResponse(url);
        }
    }
}
