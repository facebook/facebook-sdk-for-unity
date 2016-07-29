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
    /// <summary>
    /// Share dialog mode.
    /// </summary>
    public enum ShareDialogMode
    {
        // If you make changes in here make the same changes in Assets/Facebook/Editor/iOS/FBUnityInterface.h

        /// <summary>
        /// The sdk will choose which type of dialog to show
        /// See the Facebook SDKs for ios and android for specific details.
        /// </summary>
        AUTOMATIC = 0,

        /// <summary>
        /// Uses the dialog inside the native facebook applications. Note this will fail if the
        /// native applications are not installed.
        /// </summary>
        NATIVE = 1,

        /// <summary>
        /// Opens the facebook dialog in a webview.
        /// </summary>
        WEB = 2,

        /// <summary>
        /// Uses the feed dialog.
        /// </summary>
        FEED = 3,
    }
}
