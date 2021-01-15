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

    /// <summary>
    /// Classes defined on the mobile sdks.
    /// </summary>
    internal abstract class MobileFacebook : FacebookBase, IMobileFacebookImplementation
    {
        private const string CallbackIdKey = "callback_id";
        private ShareDialogMode shareDialogMode = ShareDialogMode.AUTOMATIC;

        protected MobileFacebook(CallbackManager callbackManager) : base(callbackManager)
        {
        }

        /// <summary>
        /// Gets or sets the dialog mode.
        /// </summary>
        /// <value>The dialog mode use for sharing, login, and other dialogs.</value>
        public ShareDialogMode ShareDialogMode
        {
            get
            {
                return this.shareDialogMode;
            }

            set
            {
                this.shareDialogMode = value;
                this.SetShareDialogMode(this.shareDialogMode);
            }
        }

        public abstract string UserID { get; set; }

        public abstract AuthenticationToken CurrentAuthenticationToken();

        public abstract Profile CurrentProfile();

        public abstract void UpdateUserProperties(Dictionary<string, string> parameters);

        public abstract void SetDataProcessingOptions(IEnumerable<string> options, int country, int state);

        public abstract void LoginWithTrackingPreference(
            string tracking,
            IEnumerable<string> permissions,
            string nonce,
            FacebookDelegate<ILoginResult> callback);

        public abstract void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback);

        public abstract void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback);

        public abstract bool IsImplicitPurchaseLoggingEnabled();

        public abstract void SetAutoLogAppEventsEnabled (bool autoLogAppEventsEnabled);

        public abstract void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled);

        public abstract bool SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        public abstract void SetPushNotificationsDeviceTokenString(string token);

        public override void OnLoginComplete(ResultContainer resultContainer)
        {
            var result = new LoginResult(resultContainer);
            this.OnAuthResponse(result);
        }

        public override void OnGetAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnAppRequestsComplete(ResultContainer resultContainer)
        {
            var result = new AppRequestResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnFetchDeferredAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(ResultContainer resultContainer)
        {
            var result = new ShareResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnRefreshCurrentAccessTokenComplete(ResultContainer resultContainer)
        {
            var result = new AccessTokenRefreshResult(resultContainer);
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }

            CallbackManager.OnFacebookResponse(result);
        }

        public virtual void OpenFriendFinderDialog(FacebookDelegate<IGamingServicesFriendFinderResult> callback)
        {
            throw new NotImplementedException();
        }

        public void OnFriendFinderComplete(ResultContainer resultContainer)
        {
            var result = new GamingServicesFriendFinderResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnUploadImageToMediaLibraryComplete(ResultContainer resultContainer)
        {
            var result = new MediaUploadResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnUploadVideoToMediaLibraryComplete(ResultContainer resultContainer)
        {
            var result = new MediaUploadResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnOnIAPReadyComplete(ResultContainer resultContainer)
        {
            var result = new IAPReadyResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnGetCatalogComplete(ResultContainer resultContainer)
        {
            var result = new CatalogResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnGetPurchasesComplete(ResultContainer resultContainer)
        {
            var result = new PurchasesResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnPurchaseComplete(ResultContainer resultContainer)
        {
            var result = new PurchaseResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnConsumePurchaseComplete(ResultContainer resultContainer)
        {
            var result = new ConsumePurchaseResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnInitCloudGameComplete(ResultContainer resultContainer)
        {
            var result = new InitCloudGameResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnScheduleAppToUserNotificationComplete(ResultContainer resultContainer)
        {
            var result = new ScheduleAppToUserNotificationResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnLoadInterstitialAdComplete(ResultContainer resultContainer)
        {
            var result = new InterstitialAdResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }


        public void OnShowInterstitialAdComplete(ResultContainer resultContainer)
        {
            var result = new InterstitialAdResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnLoadRewardedVideoComplete(ResultContainer resultContainer)
        {
            var result = new RewardedVideoResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnShowRewardedVideoComplete(ResultContainer resultContainer)
        {
            var result = new RewardedVideoResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnGetPayloadComplete(ResultContainer resultContainer)
        {
            var result = new PayloadResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnPostSessionScoreComplete(ResultContainer resultContainer)
        {
            var result = new SessionScoreResult(resultContainer);
        }

        public void OnOpenAppStoreComplete(ResultContainer resultContainer)
        {
            var result = new OpenAppStoreResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public virtual void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void UploadVideoToMediaLibrary(
            string caption,
            Uri videoUri,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void OnIAPReady(
            FacebookDelegate<IIAPReadyResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void GetCatalog(
            FacebookDelegate<ICatalogResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void GetPurchases(
            FacebookDelegate<IPurchasesResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void Purchase(
            string productID,
            FacebookDelegate<IPurchaseResult> callback,
            string developerPayload)
        {
            throw new NotImplementedException();
        }

        public virtual void ConsumePurchase(
            string purchaseToken,
            FacebookDelegate<IConsumePurchaseResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void InitCloudGame(
            FacebookDelegate<IInitCloudGameResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void ScheduleAppToUserNotification(
            string title,
            string body,
            Uri media,
            int timeInterval,
            string payload,
            FacebookDelegate<IScheduleAppToUserNotificationResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadInterstitialAd(
            string placementID,
            FacebookDelegate<IInterstitialAdResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void ShowInterstitialAd(
            string placementID,
            FacebookDelegate<IInterstitialAdResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadRewardedVideo(
            string placementID,
            FacebookDelegate<IRewardedVideoResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void ShowRewardedVideo(
            string placementID,
            FacebookDelegate<IRewardedVideoResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void GetPayload(
            FacebookDelegate<IPayloadResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void PostSessionScore(
            int score,
            FacebookDelegate<ISessionScoreResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void OpenAppStore(
            FacebookDelegate<IOpenAppStoreResult> callback)
        {
            throw new NotImplementedException();
        }

        protected abstract void SetShareDialogMode(ShareDialogMode mode);

        private static IDictionary<string, object> DeserializeMessage(string message)
        {
            return (Dictionary<string, object>)MiniJSON.Json.Deserialize(message);
        }

        private static string SerializeDictionary(IDictionary<string, object> dict)
        {
            return MiniJSON.Json.Serialize(dict);
        }

        private static bool TryGetCallbackId(IDictionary<string, object> result, out string callbackId)
        {
            object callback;
            callbackId = null;
            if (result.TryGetValue("callback_id", out callback))
            {
                callbackId = callback as string;
                return true;
            }

            return false;
        }

        private static bool TryGetError(IDictionary<string, object> result, out string errorMessage)
        {
            object error;
            errorMessage = null;
            if (result.TryGetValue("error", out error))
            {
                errorMessage = error as string;
                return true;
            }

            return false;
        }
    }
}
