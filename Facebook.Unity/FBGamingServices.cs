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

        /// <summary>
        /// Uploads an Image to the player's Gaming Media Library
        /// </summary>
        /// <param name="caption">Title for this image in the Media Library</param>
        /// <param name="imageUri">Path to the image file in the local filesystem. On Android
        /// this can also be a content:// URI</param>
        /// <param name="shouldLaunchMediaDialog">If we should open the Media Dialog to allow
        /// the player to Share this image right away.</param>
        /// <param name="callback">A callback for when the image upload is complete.</param>
        public static void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            MobileFacebookImpl.UploadImageToMediaLibrary(caption, imageUri, shouldLaunchMediaDialog, callback);
        }

        /// <summary>
        /// Uploads a video to the player's Gaming Media Library
        /// </summary>
        /// <param name="caption">Title for this video in the Media Library</param>
        /// <param name="videoUri">Path to the video file in the local filesystem. On Android
        /// this can also be a content:// URI</param>
        /// <param name="callback">A callback for when the video upload is complete.</param>
        /// <remarks>Note that when the callback is fired, the video will still need to be
        /// encoded before it is available in the Media Library.</remarks>
        public static void UploadVideoToMediaLibrary(
            string caption,
            Uri videoUri,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            MobileFacebookImpl.UploadVideoToMediaLibrary(caption, videoUri, callback);
        }

        public static void OnIAPReady(FacebookDelegate<IIAPReadyResult> callback) {
            MobileFacebookImpl.OnIAPReady(callback);
        }

        public static void GetCatalog(FacebookDelegate<ICatalogResult> callback) {
            MobileFacebookImpl.GetCatalog(callback);
        }

        public static void GetPurchases(FacebookDelegate<IPurchasesResult> callback) {
            MobileFacebookImpl.GetPurchases(callback);
        }

        public static void Purchase(string productID, FacebookDelegate<IPurchaseResult> callback, string developerPayload = "") {
            MobileFacebookImpl.Purchase(productID, callback, developerPayload);
        }

        public static void ConsumePurchase(string purchaseToken, FacebookDelegate<IConsumePurchaseResult> callback) {
            MobileFacebookImpl.ConsumePurchase(purchaseToken, callback);
        }

        public static void InitCloudGame(
            FacebookDelegate<IInitCloudGameResult> callback)
        {
            MobileFacebookImpl.InitCloudGame(callback);
        }

        public static void ScheduleAppToUserNotification(
            string title,
            string body,
            Uri media,
            int timeInterval,
            string payload,
            FacebookDelegate<IScheduleAppToUserNotificationResult> callback)
        {
            MobileFacebookImpl.ScheduleAppToUserNotification(title, body, media, timeInterval, payload, callback);
        }

        public static void LoadInterstitialAd(string placementID, FacebookDelegate<IInterstitialAdResult> callback) {
            MobileFacebookImpl.LoadInterstitialAd(placementID, callback);
        }

        public static void ShowInterstitialAd(string placementID, FacebookDelegate<IInterstitialAdResult> callback) {
            MobileFacebookImpl.ShowInterstitialAd(placementID, callback);
        }

        public static void LoadRewardedVideo(string placementID, FacebookDelegate<IRewardedVideoResult> callback) {
            MobileFacebookImpl.LoadRewardedVideo(placementID, callback);
        }

        public static void ShowRewardedVideo(string placementID, FacebookDelegate<IRewardedVideoResult> callback) {
            MobileFacebookImpl.ShowRewardedVideo(placementID, callback);
        }

        public static void GetPayload(FacebookDelegate<IPayloadResult> callback) {
            MobileFacebookImpl.GetPayload(callback);
        }

        public static void PostSessionScore(int score, FacebookDelegate<ISessionScoreResult> callback)
        {
            MobileFacebookImpl.PostSessionScore(score, callback);
        }

        public static void OpenAppStore(FacebookDelegate<IOpenAppStoreResult> callback) {
            MobileFacebookImpl.OpenAppStore(callback);
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
