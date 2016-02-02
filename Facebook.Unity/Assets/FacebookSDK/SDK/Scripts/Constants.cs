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
    using System.Globalization;
    using UnityEngine;

    internal static class Constants
    {
        // Callback keys
        public const string CallbackIdKey = "callback_id";
        public const string AccessTokenKey = "access_token";
        public const string UrlKey = "url";
        public const string RefKey = "ref";
        public const string ExtrasKey = "extras";
        public const string TargetUrlKey = "target_url";
        public const string CancelledKey = "cancelled";
        public const string ErrorKey = "error";

        // Callback Method Names
        public const string OnPayCompleteMethodName = "OnPayComplete";
        public const string OnShareCompleteMethodName = "OnShareLinkComplete";
        public const string OnAppRequestsCompleteMethodName = "OnAppRequestsComplete";
        public const string OnGroupCreateCompleteMethodName = "OnGroupCreateComplete";
        public const string OnGroupJoinCompleteMethodName = "OnJoinGroupComplete";

        // Graph API
        public const string GraphApiVersion = "v2.5";
        public const string GraphUrlFormat = "https://graph.{0}/{1}/";

        // Permission Strings
        public const string UserLikesPermission = "user_likes";
        public const string EmailPermission = "email";
        public const string PublishActionsPermission = "publish_actions";
        public const string PublishPagesPermission = "publish_pages";

        // The current platform. We save this in a varriable to allow for
        // mocking during testing
        private static FacebookUnityPlatform? currentPlatform;

        /// <summary>
        /// Gets the graph URL.
        /// </summary>
        /// <value>The graph URL. Ex. https://graph.facebook.com/v2.5/</value>
        public static Uri GraphUrl
        {
            get
            {
                string urlStr = string.Format(
                    CultureInfo.InvariantCulture,
                    Constants.GraphUrlFormat,
                    FB.FacebookDomain,
                    FB.GraphApiVersion);
                return new Uri(urlStr);
            }
        }

        public static string GraphApiUserAgent
        {
            get
            {
                // Return the Unity SDK User Agent and our platform user agent
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    FB.FacebookImpl.SDKUserAgent,
                    Constants.UnitySDKUserAgent);
            }
        }

        public static bool IsMobile
        {
            get
            {
                return Constants.CurrentPlatform == FacebookUnityPlatform.Android ||
                    Constants.CurrentPlatform == FacebookUnityPlatform.IOS;
            }
        }

        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsWeb
        {
            get
            {
                return Constants.CurrentPlatform == FacebookUnityPlatform.WebGL ||
                    Constants.CurrentPlatform == FacebookUnityPlatform.WebPlayer;
            }
        }

        /// <summary>
        /// Returns the legacy user agent suffix that gets
        /// appended to graph requests on ios and android.
        /// </summary>
        /// <value>The user agent unity suffix legacy.</value>
        public static string UnitySDKUserAgentSuffixLegacy
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "Unity.{0}",
                    FacebookSdkVersion.Build);
            }
        }

        /// <summary>
        /// Gets the Unity SDK user agent.
        /// </summary>
        public static string UnitySDKUserAgent
        {
            get
            {
                return Utilities.GetUserAgent("FBUnitySDK", FacebookSdkVersion.Build);
            }
        }

        public static bool DebugMode
        {
            get
            {
                return Debug.isDebugBuild;
            }
        }

        public static FacebookUnityPlatform CurrentPlatform
        {
            get
            {
                if (!Constants.currentPlatform.HasValue)
                {
                    Constants.currentPlatform = Constants.GetCurrentPlatform();
                }

                return Constants.currentPlatform.Value;
            }

            set
            {
                Constants.currentPlatform = value;
            }
        }

        private static FacebookUnityPlatform GetCurrentPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return FacebookUnityPlatform.Android;
                case RuntimePlatform.IPhonePlayer:
                    return FacebookUnityPlatform.IOS;
                case RuntimePlatform.WebGLPlayer:
                    return FacebookUnityPlatform.WebGL;
                case RuntimePlatform.WindowsWebPlayer:
                case RuntimePlatform.OSXWebPlayer:
                    return FacebookUnityPlatform.WebPlayer;
                default:
                    return FacebookUnityPlatform.Unknown;
            }
        }
    }
}
