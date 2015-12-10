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

    internal class CanvasJSWrapper : ICanvasJSWrapper
    {
        // The source code for our js sdk binding.
        private const string JSSDKBindingFileName = "JSSDKBindings";

        public string IntegrationMethodJs
        {
            get
            {
                TextAsset ta = Resources.Load(JSSDKBindingFileName) as TextAsset;
                if (ta)
                {
                    return ta.text;
                }

                return null;
            }
        }

        public string GetSDKVersion()
        {
            return Constants.GraphAPIVersion;
        }

        public void ExternalCall(string functionName, params object[] args)
        {
            Application.ExternalCall(functionName, args);
        }

        public void ExternalEval(string script)
        {
            Application.ExternalEval(script);
        }

        public void DisableFullScreen()
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
        }
    }
}
