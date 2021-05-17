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

namespace Facebook.Unity.Mobile
{
    using System;
    using System.Collections.Generic;

    internal interface IMobileFacebook : IFacebook
    {
        ShareDialogMode ShareDialogMode { get; set; }

        string UserID { get; set; }

        void UpdateUserProperties(Dictionary<string, string> parameters);

        void EnableProfileUpdatesOnAccessTokenChange(bool enable);

        void LoginWithTrackingPreference(string tracking, IEnumerable<string> permissions, string nonce,
            FacebookDelegate<ILoginResult> callback);

        void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback);

        void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback);

        bool IsImplicitPurchaseLoggingEnabled();

        void SetPushNotificationsDeviceTokenString(string token);

        void SetAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled);

        void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled);

        bool SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        void SetDataProcessingOptions(IEnumerable<string> options, int country, int state);

        void OpenFriendFinderDialog(FacebookDelegate<IGamingServicesFriendFinderResult> callback);

        void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback);

        void UploadVideoToMediaLibrary(
            string caption,
            Uri videoUri,
            FacebookDelegate<IMediaUploadResult> callback);

        void OnIAPReady(FacebookDelegate<IIAPReadyResult> callback);

        void GetCatalog(FacebookDelegate<ICatalogResult> callback);

        void GetPurchases(FacebookDelegate<IPurchasesResult> callback);

        void Purchase(string productID, FacebookDelegate<IPurchaseResult> callback, string developPayload);

        void ConsumePurchase(string productToken, FacebookDelegate<IConsumePurchaseResult> callback);

        void InitCloudGame(FacebookDelegate<IInitCloudGameResult> callback);

        void ScheduleAppToUserNotification(
            string title,
            string body,
            Uri media,
            int timeInterval,
            string payload,
            FacebookDelegate<IScheduleAppToUserNotificationResult> callback);

        void LoadInterstitialAd(string placementID, FacebookDelegate<IInterstitialAdResult> callback);

        void ShowInterstitialAd(string placementID, FacebookDelegate<IInterstitialAdResult> callback);

        void LoadRewardedVideo(string placementID, FacebookDelegate<IRewardedVideoResult> callback);

        void ShowRewardedVideo(string placementID, FacebookDelegate<IRewardedVideoResult> callback);

        void GetPayload(FacebookDelegate<IPayloadResult> callback);

        void PostSessionScore(int score, FacebookDelegate<ISessionScoreResult> callback);

        void PostTournamentScore(int score, FacebookDelegate<ITournamentScoreResult> callback);

        void GetTournament(FacebookDelegate<ITournamentResult> callback);

        void ShareTournament(Dictionary<string, string> data, FacebookDelegate<ITournamentScoreResult> callback);

        void CreateTournament(
            int initialScore,
            string title,
            string imageBase64DataUrl,
            string sortOrder,
            string scoreFormat,
            Dictionary<string, string> data,
            FacebookDelegate<ITournamentResult> callback);

        void OpenAppStore(FacebookDelegate<IOpenAppStoreResult> callback);

        AuthenticationToken CurrentAuthenticationToken();

        Profile CurrentProfile();
    }
}
