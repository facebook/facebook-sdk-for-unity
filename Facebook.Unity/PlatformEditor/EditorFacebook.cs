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

namespace Facebook.Unity.Editor
{
    using System;
    using System.Collections.Generic;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Editor.Dialogs;
    using Facebook.Unity.Mobile;

    internal class EditorFacebook : FacebookBase, IMobileFacebookImplementation, ICanvasFacebookImplementation
    {
        private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. " +
            "Behavior may not be the same as when used on iOS, Android, or Web.";

        private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";

        private IEditorWrapper editorWrapper;

        public EditorFacebook(IEditorWrapper wrapper, CallbackManager callbackManager) : base(callbackManager)
        {
            this.editorWrapper = wrapper;
        }

        public EditorFacebook() : this(new EditorWrapper(EditorFacebook.EditorGameObject), new CallbackManager())
        {
        }

        public override bool LimitEventUsage { get; set; }

        public ShareDialogMode ShareDialogMode { get; set; }

        public override string SDKName
        {
            get
            {
                return "FBUnityEditorSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return Facebook.Unity.FacebookSdkVersion.Build;
            }
        }

        public string UserID { get; set; }

        public void UpdateUserProperties(Dictionary<string, string> parameters)
        {
            return;
        }

        private static IFacebookCallbackHandler EditorGameObject
        {
            get
            {
                return ComponentFactory.GetComponent<EditorFacebookGameObject>();
            }
        }

        public override void Init(InitDelegate onInitComplete)
        {
            // Warn that editor behavior will not match supported platforms
            FacebookLogger.Warn(WarningMessage);

            base.Init(onInitComplete);
            this.editorWrapper.Init();
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            // For the editor don't worry about the difference between
            // LogInWithReadPermissions and LogInWithPublishPermissions
            this.LogInWithPublishPermissions(permissions, callback);
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.editorWrapper.ShowLoginMockDialog(
                this.OnLoginComplete,
                this.CallbackManager.AddFacebookDelegate(callback),
                permissions.ToCommaSeparateList());
        }

        public void EnableProfileUpdatesOnAccessTokenChange(bool enable)
        {
            FacebookLogger.Log("Pew! Pretending to enable Profile updates on access token change.  Doesn't actually work in the editor");
        }

        public void LoginWithTrackingPreference(
            string tracking,
            IEnumerable<string> permissions,
            string nonce,
            FacebookDelegate<ILoginResult> callback)
        {
            this.editorWrapper.ShowLoginMockDialog(
                this.OnLoginComplete,
                this.CallbackManager.AddFacebookDelegate(callback),
                permissions.ToCommaSeparateList());
        }

        public override void AppRequest(
            string message,
            OGActionType? actionType,
            string objectId,
            IEnumerable<string> to,
            IEnumerable<object> filters,
            IEnumerable<string> excludeIds,
            int? maxRecipients,
            string data,
            string title,
            FacebookDelegate<IAppRequestResult> callback)
        {
            this.editorWrapper.ShowAppRequestMockDialog(
                this.OnAppRequestsComplete,
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            this.editorWrapper.ShowMockShareDialog(
                this.OnShareLinkComplete,
                "ShareLink",
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public override void FeedShare(
            string toId,
            Uri link,
            string linkName,
            string linkCaption,
            string linkDescription,
            Uri picture,
            string mediaSource,
            FacebookDelegate<IShareResult> callback)
        {
            this.editorWrapper.ShowMockShareDialog(
                this.OnShareLinkComplete,
                "FeedShare",
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public override void ActivateApp(string appId)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            var result = new Dictionary<string, object>();
            result[Constants.UrlKey] = "mockurl://testing.url";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
            this.OnGetAppLinkComplete(new ResultContainer(result));
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public bool IsImplicitPurchaseLoggingEnabled()
        {
            return true;
        }

        public void SetAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled)
        {
            return;
        }

        public void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled)
        {
            return;
        }

        public bool SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            return true;
        }

        public void SetPushNotificationsDeviceTokenString(string token)
        {
            return;
        }

        public void SetDataProcessingOptions(IEnumerable<string> options, int country, int state)
        {
            return;
        }

        public AuthenticationToken CurrentAuthenticationToken()
        {
            return null;
        }

        public Profile CurrentProfile()
        {
            return null;
        }

        public void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            var result = new Dictionary<string, object>();
            result[Constants.UrlKey] = "mockurl://testing.url";
            result[Constants.RefKey] = "mock ref";
            result[Constants.ExtrasKey] = new Dictionary<string, object>()
            {
                {
                    "mock extra key", "mock extra value"
                }
            };

            result[Constants.TargetUrlKey] = "mocktargeturl://mocktarget.url";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
            this.OnFetchDeferredAppLinkComplete(new ResultContainer(result));
        }

        public void Pay(
            string product,
            string action,
            int quantity,
            int? quantityMin,
            int? quantityMax,
            string requestId,
            string pricepointId,
            string testCurrency,
            FacebookDelegate<IPayResult> callback)
        {
            this.editorWrapper.ShowPayMockDialog(
                this.OnPayComplete,
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public void PayWithProductId(
            string productId,
            string action,
            int quantity,
            int? quantityMin,
            int? quantityMax,
            string requestId,
            string pricepointId,
            string testCurrency,
            FacebookDelegate<IPayResult> callback)
        {
            this.editorWrapper.ShowPayMockDialog(
                this.OnPayComplete,
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public void PayWithProductId(
            string productId,
            string action,
            string developerPayload,
            string testCurrency,
            FacebookDelegate<IPayResult> callback)
        {
            this.editorWrapper.ShowPayMockDialog(
                this.OnPayComplete,
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback)
        {
            if (callback == null)
            {
                return;
            }

            var result = new Dictionary<string, object>()
            {
                { Constants.CallbackIdKey, this.CallbackManager.AddFacebookDelegate(callback) }
            };

            if (AccessToken.CurrentAccessToken == null)
            {
                result[Constants.ErrorKey] = "No current access token";
            }
            else
            {
                var accessTokenDic = (IDictionary<string, object>)MiniJSON.Json.Deserialize(
                    AccessToken.CurrentAccessToken.ToJson());

                result.AddAllKVPFrom(accessTokenDic);
            }

            this.OnRefreshCurrentAccessTokenComplete(new ResultContainer(result));
        }

        public override void OnAppRequestsComplete(ResultContainer resultContainer)
        {
            var result = new AppRequestResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGetAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnLoginComplete(ResultContainer resultContainer)
        {
            var result = new LoginResult(resultContainer);
            this.OnAuthResponse(result);
        }

        public override void OnShareLinkComplete(ResultContainer resultContainer)
        {
            var result = new ShareResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnFetchDeferredAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnPayComplete(ResultContainer resultContainer)
        {
            var result = new PayResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnRefreshCurrentAccessTokenComplete(ResultContainer resultContainer)
        {
            var result = new AccessTokenRefreshResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
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
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnPostTournamentScoreComplete(ResultContainer resultContainer)
        {
            var result = new TournamentScoreResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnGetTournamentComplete(ResultContainer resultContainer)
        {
            var result = new TournamentResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnShareTournamentComplete(ResultContainer resultContainer)
        {
            var result = new TournamentScoreResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnCreateTournamentComplete(ResultContainer resultContainer)
        {
            var result = new TournamentResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnOpenAppStoreComplete(ResultContainer resultContainer)
        {
            var result = new OpenAppStoreResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OpenFriendFinderDialog(FacebookDelegate<IGamingServicesFriendFinderResult> callback)
        {
            this.editorWrapper.ShowMockFriendFinderDialog(
                this.OnFriendFinderComplete,
                "Friend Finder Dialog",
                this.CallbackManager.AddFacebookDelegate(callback));
        }

        public void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["id"] = "1232453";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
            this.OnFetchDeferredAppLinkComplete(new ResultContainer(result));
        }

        public void UploadVideoToMediaLibrary(
            string caption,
            Uri imageUri,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            // construct a dummy ResultContainer
            // to pretend we actually did an upload
            var result = new Dictionary<string, object>();
            result["video_id"] = "456789";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
            this.OnFetchDeferredAppLinkComplete(new ResultContainer(result));
        }

        public void OnIAPReady(FacebookDelegate<IIAPReadyResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void GetCatalog(FacebookDelegate<ICatalogResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "[{\"productID\":\"123\",\"title\":\"item\",\"price\":\"$0.99\",\"priceCurrencyCode\":\"USD\"}]";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void GetPurchases(FacebookDelegate<IPurchasesResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "[{\"isConsumed\":\"false\",\"paymentID\":\"2607915835989565\",\"productID\":\"123\",\"purchaseTime\":\"1583797821\":\"purchaseToken\":\"1655700687901784\",\"signedRequest\":\"abc123ZYZ\"}]";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void Purchase(string productID, FacebookDelegate<IPurchaseResult> callback, string developerPayload = "")
        {
            var result = new Dictionary<string, object>();
            result["success"] = "{\"isConsumed\":\"false\",\"paymentID\":\"2607915835989565\",\"productID\":\"123\",\"purchaseTime\":\"1583797821\",\"purchaseToken\":\"1655700687901784\",\"signedRequest\":\"XZZ9xQDHOGulfhZMRVQ8UC-TadAqFrueYveAAqxock.eyJhbGdvcm10aG0iOiJITUFDLVNIQTI1Nilslm...\"}";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void ConsumePurchase(string productID, FacebookDelegate<IConsumePurchaseResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void InitCloudGame(FacebookDelegate<IInitCloudGameResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void ScheduleAppToUserNotification(
            string title,
            string body,
            Uri media,
            int timeInterval,
            string payload,
            FacebookDelegate<IScheduleAppToUserNotificationResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void LoadInterstitialAd(string placementID, FacebookDelegate<IInterstitialAdResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void ShowInterstitialAd(string placementID, FacebookDelegate<IInterstitialAdResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void LoadRewardedVideo(string placementID, FacebookDelegate<IRewardedVideoResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void ShowRewardedVideo(string placementID, FacebookDelegate<IRewardedVideoResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void GetPayload(FacebookDelegate<IPayloadResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "{\"key\":\"test\",\"value\":\"123\"}";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void PostSessionScore(int score, FacebookDelegate<ISessionScoreResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void PostTournamentScore(int score, FacebookDelegate<ITournamentScoreResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void GetTournament(FacebookDelegate<ITournamentResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["tournamentId"] = "123";
            result["contextId"] = "456";
            result["endTime"] = "456";
            result["data"] = new Dictionary<string, string>();
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void ShareTournament(Dictionary<string, string> data, FacebookDelegate<ITournamentResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void CreateTournament(
            int initialScore,
            string title,
            string imageBase64DataUrl,
            Dictionary<string, string> data,
            FacebookDelegate<ITournamentResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["tournamentId"] = "123";
            result["contextId"] = "456";
            result["endTime"] = "456";
            result["data"] = new Dictionary<string, string>();
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        public void OpenAppStore(FacebookDelegate<IOpenAppStoreResult> callback)
        {
            var result = new Dictionary<string, object>();
            result["success"] = "";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
        }

        #region Canvas Dummy Methods

        public void OnFacebookAuthResponseChange(ResultContainer resultContainer)
        {
            throw new NotSupportedException();
        }

        public void OnUrlResponse(string message)
        {
            throw new NotSupportedException();
        }

        public void OnHideUnity(bool hidden)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
