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
    using System.Collections.Generic;
    using System.Globalization;
    using Facebook.Unity.Gameroom;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Editor;
    using Facebook.Unity.Mobile;
    using Facebook.Unity.Mobile.Android;
    using Facebook.Unity.Mobile.IOS;
    using Facebook.Unity.Settings;
    using UnityEngine;

    /// <summary>
    /// Static class for exposing the Facebook GamingServices Integration.
    /// </summary>
    public sealed class FBGamingServices : ScriptableObject
    {
        /// <summary>
        /// Opens the Friend Finder Dialog
        /// </summary>
        /// <param name="callback">A callback for when the Dialog is closed.</param>
        public static void OpenFriendFinderDialog(FacebookDelegate<IGamingServicesFriendFinderResult> callback)
        {
            MobileFacebookImpl.OpenFriendFinderDialog(callback);
        }

        private static IMobileFacebook MobileFacebookImpl
        {
            get
            {
                IMobileFacebook impl = FB.FacebookImpl as IMobileFacebook;
                if (impl == null)
                {
                    throw new InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
                }

                return impl;
            }
        }
    }
}
