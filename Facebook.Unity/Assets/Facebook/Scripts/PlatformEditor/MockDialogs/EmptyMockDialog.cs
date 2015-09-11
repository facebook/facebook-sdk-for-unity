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

namespace Facebook.Unity.Editor.Dialogs
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class EmptyMockDialog : EditorFacebookMockDialog
    {
        public string EmptyDialogTitle { get; set; }

        protected override float WindowHeight
        {
            get
            {
                return 500;
            }
        }

        protected override string DialogTitle
        {
            get
            {
                return this.EmptyDialogTitle;
            }
        }

        protected override void DoGui()
        {
            // Empty
        }

        protected override void SendSuccessResult()
        {
            var result = new Dictionary<string, object>();
            result["did_complete"] = true;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                result[Constants.CallbackIdKey] = this.CallbackID;
            }

            if (this.Callback != null)
            {
                this.Callback(MiniJSON.Json.Serialize(result));
            }
        }
    }
}
