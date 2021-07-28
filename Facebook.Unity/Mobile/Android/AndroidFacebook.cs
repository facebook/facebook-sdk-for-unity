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

namespace Facebook.Unity.Mobile.Android
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System.Reflection;
    using UnityEngine;

    internal sealed class AndroidFacebook : MobileFacebook
    {
        public const string LoginPermissionsKey = "scope";

        // This class holds all the of the wrapper methods that we call into
        private bool limitEventUsage;
        private IAndroidWrapper androidWrapper;
        private string userID;

        public AndroidFacebook() : this(GetAndroidWrapper(), new CallbackManager())
        {
        }

        public AndroidFacebook(IAndroidWrapper androidWrapper, CallbackManager callbackManager)
            : base(callbackManager)
        {
            this.KeyHash = string.Empty;
            this.androidWrapper = androidWrapper;
        }

        // key Hash used for Android SDK
        public string KeyHash { get; private set; }

        public override bool LimitEventUsage
        {
            get
            {
                return this.limitEventUsage;
            }

            set
            {
                this.limitEventUsage = value;
                this.CallFB("SetLimitEventUsage", value.ToString());
            }
        }

        public override string UserID
        {
            get
            {
                return userID;
            }

            set
            {
                this.userID = value;
                this.CallFB("SetUserID", value);
            }
        }

        public override void UpdateUserProperties(Dictionary<string, string> parameters)
        {
            var args = new MethodArguments();
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                args.AddString(entry.Key, entry.Value);
            }
            this.CallFB("UpdateUserProperties", args.ToJsonString());
        }

        public override void SetDataProcessingOptions(IEnumerable<string> options, int country, int state)
        {
            var args = new MethodArguments();
            args.AddList<string>("options", options);
            args.AddPrimative<int>("country", country);
            args.AddPrimative<int>("state", state);
            this.CallFB("SetDataProcessingOptions", args.ToJsonString());
        }

        public override void SetAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled)
        {
            this.CallFB("SetAutoLogAppEventsEnabled", autoLogAppEventsEnabled.ToString());
        }

        public override void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled)
        {
            this.CallFB("SetAdvertiserIDCollectionEnabled", advertiserIDCollectionEnabled.ToString());
        }

        public override bool SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            return false;
        }

        public override void SetPushNotificationsDeviceTokenString(string token)
        {
            this.CallFB("SetPushNotificationsDeviceTokenString", token);
        }

        public override string SDKName
        {
            get
            {
                return "FBAndroidSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return this.androidWrapper.CallStatic<string>("GetSdkVersion");
            }
        }

        public void Init(
            string appId,
            HideUnityDelegate hideUnityDelegate,
            InitDelegate onInitComplete)
        {
            // Set the user agent suffix for graph requests
            // This should be set before a call to init to ensure that
            // requests made during init include this suffix.
            this.CallFB(
                "SetUserAgentSuffix",
                string.Format(Constants.UnitySDKUserAgentSuffixLegacy));

            base.Init(onInitComplete);

            var args = new MethodArguments();
            args.AddString("appId", appId);
            var initCall = new JavaMethodCall<IResult>(this, "Init");
            initCall.Call(args);
            this.userID = this.androidWrapper.CallStatic<string>("GetUserID");
        }

        public override void EnableProfileUpdatesOnAccessTokenChange(bool enable)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("This function is only implemented on iOS.");
            }
            return;
        }

        public override void LoginWithTrackingPreference(
            string tracking,
            IEnumerable<string> permissions,
            string nonce,
            FacebookDelegate<ILoginResult> callback)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("This function is only implemented on iOS. Please use .LoginWithReadPermissions() or .LoginWithPublishPermissions() on other platforms.");
            }
            return;
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddCommaSeparatedList(AndroidFacebook.LoginPermissionsKey, permissions);
            var loginCall = new JavaMethodCall<ILoginResult>(this, "LoginWithReadPermissions");
            loginCall.Callback = callback;
            loginCall.Call(args);
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddCommaSeparatedList(AndroidFacebook.LoginPermissionsKey, permissions);
            var loginCall = new JavaMethodCall<ILoginResult>(this, "LoginWithPublishPermissions");
            loginCall.Callback = callback;
            loginCall.Call(args);
        }

        public override void LogOut()
        {
            base.LogOut();
            var logoutCall = new JavaMethodCall<IResult>(this, "Logout");
            logoutCall.Call();
        }

        public override AuthenticationToken CurrentAuthenticationToken()
        {
            return null;
        }

        public override Profile CurrentProfile()
        {
            String profileString = this.androidWrapper.CallStatic<string>("GetCurrentProfile");
            if (!String.IsNullOrEmpty(profileString))
            {
                try
                {
                    IDictionary<string, string> profile = Utilities.ParseStringDictionaryFromString(profileString);
                    string id;
                    string firstName;
                    string middleName;
                    string lastName;
                    string name;
                    string email;
                    string imageURL;
                    string linkURL;
                    string friendIDs;
                    string birthday;
                    string gender;
                    profile.TryGetValue("userID", out id);
                    profile.TryGetValue("firstName", out firstName);
                    profile.TryGetValue("middleName", out middleName);
                    profile.TryGetValue("lastName", out lastName);
                    profile.TryGetValue("name", out name);
                    profile.TryGetValue("email", out email);
                    profile.TryGetValue("imageURL", out imageURL);
                    profile.TryGetValue("linkURL", out linkURL);
                    profile.TryGetValue("friendIDs", out friendIDs);
                    profile.TryGetValue("birthday", out birthday);
                    profile.TryGetValue("gender", out gender);

                    UserAgeRange ageRange = UserAgeRange.AgeRangeFromDictionary(profile);
                    FBLocation hometown = FBLocation.FromDictionary("hometown", profile);
                    FBLocation location = FBLocation.FromDictionary("location", profile);
                    return new Profile(
                        userID,
                        firstName,
                        middleName,
                        lastName,
                        name,
                        email,
                        imageURL,
                        linkURL,
                        friendIDs?.Split(','),
                        birthday,
                        ageRange,
                        hometown,
                        location,
                        gender);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public void RetrieveLoginStatus(FacebookDelegate<ILoginStatusResult> callback) {
            var loginCall = new JavaMethodCall<ILoginStatusResult>(this, "RetrieveLoginStatus");
            loginCall.Callback = callback;
            loginCall.Call();
        }

        public void OnLoginStatusRetrieved(ResultContainer resultContainer)
        {
            var result = new LoginStatusResult(resultContainer);
            this.OnAuthResponse(result);
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
            this.ValidateAppRequestArgs(
                message,
                actionType,
                objectId,
                to,
                filters,
                excludeIds,
                maxRecipients,
                data,
                title,
                callback);

            MethodArguments args = new MethodArguments();
            args.AddString("message", message);
            args.AddNullablePrimitive("action_type", actionType);
            args.AddString("object_id", objectId);
            args.AddCommaSeparatedList("to", to);
            if (filters != null && filters.Any())
            {
                string mobileFilter = filters.First() as string;
                if (mobileFilter != null)
                {
                    args.AddString("filters", mobileFilter);
                }
            }

            args.AddNullablePrimitive("max_recipients", maxRecipients);
            args.AddString("data", data);
            args.AddString("title", title);
            var appRequestCall = new JavaMethodCall<IAppRequestResult>(this, "AppRequest");
            appRequestCall.Callback = callback;
            appRequestCall.Call(args);
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddUri("content_url", contentURL);
            args.AddString("content_title", contentTitle);
            args.AddString("content_description", contentDescription);
            args.AddUri("photo_url", photoURL);
            var shareLinkCall = new JavaMethodCall<IShareResult>(this, "ShareLink");
            shareLinkCall.Callback = callback;
            shareLinkCall.Call(args);
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
            MethodArguments args = new MethodArguments();
            args.AddString("toId", toId);
            args.AddUri("link", link);
            args.AddString("linkName", linkName);
            args.AddString("linkCaption", linkCaption);
            args.AddString("linkDescription", linkDescription);
            args.AddUri("picture", picture);
            args.AddString("mediaSource", mediaSource);
            var call = new JavaMethodCall<IShareResult>(this, "FeedShare");
            call.Callback = callback;
            call.Call(args);
        }

        public override void GetAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            var getAppLink = new JavaMethodCall<IAppLinkResult>(this, "GetAppLink");
            getAppLink.Callback = callback;
            getAppLink.Call();
        }

        public void ClearAppLink()
        {
          this.CallFB("ClearAppLink", null);
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("logEvent", logEvent);
            args.AddString("valueToSum", valueToSum?.ToString(CultureInfo.InvariantCulture));
            args.AddDictionary("parameters", parameters);
            var appEventcall = new JavaMethodCall<IResult>(this, "LogAppEvent");
            appEventcall.Call(args);
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("logPurchase", logPurchase.ToString(CultureInfo.InvariantCulture));
            args.AddString("currency", currency);
            args.AddDictionary("parameters", parameters);
            var logPurchaseCall = new JavaMethodCall<IResult>(this, "LogAppEvent");
            logPurchaseCall.Call(args);
        }

        public override bool IsImplicitPurchaseLoggingEnabled()
        {
            return this.androidWrapper.CallStatic<bool>("IsImplicitPurchaseLoggingEnabled");
        }

        public override void ActivateApp(string appId)
        {
            this.CallFB("ActivateApp", null);
        }

        public override void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            MethodArguments args = new MethodArguments();
            var fetchDeferredAppLinkData = new JavaMethodCall<IAppLinkResult>(this, "FetchDeferredAppLinkData");
            fetchDeferredAppLinkData.Callback = callback;
            fetchDeferredAppLinkData.Call(args);
        }

        public override void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback)
        {
            var refreshCurrentAccessToken = new JavaMethodCall<IAccessTokenRefreshResult>(
                this,
                "RefreshCurrentAccessToken");
            refreshCurrentAccessToken.Callback = callback;
            refreshCurrentAccessToken.Call();
        }

        public override void OpenFriendFinderDialog(
            FacebookDelegate<IGamingServicesFriendFinderResult> callback)
        {
            var openFriendFinderDialog = new JavaMethodCall<IGamingServicesFriendFinderResult>(
                this,
                "OpenFriendFinderDialog")
            {
                Callback = callback
            };
            openFriendFinderDialog.Call();
        }

        public override void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("caption", caption);
            args.AddUri("imageUri", imageUri);
            args.AddString("shouldLaunchMediaDialog", shouldLaunchMediaDialog.ToString());
            var uploadImageToMediaLibrary = new JavaMethodCall<IMediaUploadResult>(
                this,
                "UploadImageToMediaLibrary")
            {
                Callback = callback
            };
            uploadImageToMediaLibrary.Call(args);
        }

        public override void UploadVideoToMediaLibrary(
            string caption,
            Uri videoUri,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("caption", caption);
            args.AddUri("videoUri", videoUri);
            var uploadImageToMediaLibrary = new JavaMethodCall<IMediaUploadResult>(
                this,
                "UploadVideoToMediaLibrary")
            {
                Callback = callback
            };
            uploadImageToMediaLibrary.Call(args);
        }

        public override void OnIAPReady(
            FacebookDelegate<IIAPReadyResult> callback)
        {
            var onIAPReady = new JavaMethodCall<IIAPReadyResult>(
                this,
                "OnIAPReady")
            {
                Callback = callback
            };
            onIAPReady.Call();
        }

        public override void GetCatalog(
            FacebookDelegate<ICatalogResult> callback)
        {
            var getCatalog = new JavaMethodCall<ICatalogResult>(
                this,
                "GetCatalog")
            {
                Callback = callback
            };
            getCatalog.Call();
        }

        public override void GetPurchases(
            FacebookDelegate<IPurchasesResult> callback)
        {
            var getPurchases = new JavaMethodCall<IPurchasesResult>(
                this,
                "GetPurchases")
            {
                Callback = callback
            };
            getPurchases.Call();
        }

        public override void Purchase(
            string productID,
            FacebookDelegate<IPurchaseResult> callback,
            string developerPayload = "")
        {
            MethodArguments args = new MethodArguments();
            args.AddString("productID", productID);
            args.AddString("developerPayload", developerPayload);
            var purchase = new JavaMethodCall<IPurchaseResult>(
                this,
                "Purchase")
            {
                Callback = callback
            };
            purchase.Call(args);
        }

        public override void ConsumePurchase(
            string purchaseToken,
            FacebookDelegate<IConsumePurchaseResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("purchaseToken", purchaseToken);
            var consumePurchase = new JavaMethodCall<IConsumePurchaseResult>(
                this,
                "ConsumePurchase")
            {
                Callback = callback
            };
            consumePurchase.Call(args);
        }

        public override void InitCloudGame(
            FacebookDelegate<IInitCloudGameResult> callback)
        {
            var initCloudGame = new JavaMethodCall<IInitCloudGameResult>(
                this,
                "InitCloudGame")
            {
                Callback = callback
            };
            initCloudGame.Call();
        }

        public override void ScheduleAppToUserNotification(
            string title,
            string body,
            Uri media,
            int timeInterval,
            string payload,
            FacebookDelegate<IScheduleAppToUserNotificationResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("title", title);
            args.AddString("body", body);
            args.AddUri("media", media);
            args.AddPrimative("timeInterval", timeInterval);
            args.AddString("payload", payload);
            var scheduleAppToUserNotification = new JavaMethodCall<IScheduleAppToUserNotificationResult>(
                this,
                "ScheduleAppToUserNotification")
            {
                Callback = callback
            };
            scheduleAppToUserNotification.Call(args);
        }

        public override void LoadInterstitialAd(
            string placementID,
            FacebookDelegate<IInterstitialAdResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("placementID", placementID);
            var loadInterstitialAd = new JavaMethodCall<IInterstitialAdResult>(
                this,
                "LoadInterstitialAd")
            {
                Callback = callback
            };
            loadInterstitialAd.Call(args);
        }

        public override void ShowInterstitialAd(
            string placementID,
            FacebookDelegate<IInterstitialAdResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("placementID", placementID);
            var showInterstitialAd = new JavaMethodCall<IInterstitialAdResult>(
                this,
                "ShowInterstitialAd")
            {
                Callback = callback
            };
            showInterstitialAd.Call(args);
        }

        public override void LoadRewardedVideo(
            string placementID,
            FacebookDelegate<IRewardedVideoResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("placementID", placementID);
            var loadRewardedVideo = new JavaMethodCall<IRewardedVideoResult>(
                this,
                "LoadRewardedVideo")
            {
                Callback = callback
            };
            loadRewardedVideo.Call(args);
        }

        public override void ShowRewardedVideo(
            string placementID,
            FacebookDelegate<IRewardedVideoResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("placementID", placementID);
            var showRewardedVideo = new JavaMethodCall<IRewardedVideoResult>(
                this,
                "ShowRewardedVideo")
            {
                Callback = callback
            };
            showRewardedVideo.Call(args);
        }

        public override void GetPayload(
            FacebookDelegate<IPayloadResult> callback)
        {
            var getPayload = new JavaMethodCall<IPayloadResult>(
                this,
                "GetPayload")
            {
                Callback = callback
            };
            getPayload.Call();
        }

        public override void PostSessionScore(int score, FacebookDelegate<ISessionScoreResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("score", score.ToString());
            var postSessionScore = new JavaMethodCall<ISessionScoreResult>(
                this,
                "PostSessionScore")
            {
                Callback = callback
            };
            postSessionScore.Call(args);
        }

        public override void PostTournamentScore(int score, FacebookDelegate<ITournamentScoreResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("score", score.ToString());

            var postTournamentScore = new JavaMethodCall<ITournamentScoreResult>(
                this,
                "PostTournamentScore")
            {
                Callback = callback
            };
            postTournamentScore.Call(args);
        }

        public override void GetTournament(FacebookDelegate<ITournamentResult> callback)
        {
            var getTournament = new JavaMethodCall<ITournamentResult>(
                this,
                "GetTournament")
            {
                Callback = callback
            };
            getTournament.Call();
        }

        public override void CreateTournament(
            int initialScore,
            string title,
            string imageBase64DataUrl,
            string sortOrder,
            string scoreFormat,
            Dictionary<string, string> data,
            FacebookDelegate<ITournamentResult> callback)
        {
            MethodArguments args = new MethodArguments();

            args.AddString("initialScore", initialScore.ToString());
            args.AddString("title", title);
            args.AddString("imageBase64DataUrl", imageBase64DataUrl);
            args.AddString("sortOrder", sortOrder);
            args.AddString("scoreFormat", scoreFormat);
            args.AddDictionary("data", data.ToDictionary( pair => pair.Key, pair => (object) pair.Value));
            var createTournament = new JavaMethodCall<ITournamentResult>(
                this,
                "CreateTournament")
            {
                Callback = callback
            };
            createTournament.Call(args);
        }

        public override void ShareTournament(Dictionary<string, string> data, FacebookDelegate<ITournamentScoreResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddDictionary("data", data.ToDictionary(pair => pair.Key, pair => (object)pair.Value));

            var shareTournament = new JavaMethodCall<ITournamentScoreResult>(
                this,
                "ShareTournament")
            {
                Callback = callback
            };
            shareTournament.Call(args);
        }

        public override void OpenAppStore(
            FacebookDelegate<IOpenAppStoreResult> callback)
        {
            var openAppStore = new JavaMethodCall<IOpenAppStoreResult>(
                this,
                "OpenAppStore")
            {
                Callback = callback
            };
            openAppStore.Call();
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            this.CallFB("SetShareDialogMode", mode.ToString());
        }

        private static IAndroidWrapper GetAndroidWrapper()
        {
            Assembly assembly = Assembly.Load("Facebook.Unity.Android");
            Type type = assembly.GetType("Facebook.Unity.Android.AndroidWrapper");
            IAndroidWrapper javaClass = (IAndroidWrapper)Activator.CreateInstance(type);
            return javaClass;
        }

        private void CallFB(string method, string args)
        {
            this.androidWrapper.CallStatic(method, args);
        }

        private class JavaMethodCall<T> : MethodCall<T> where T : IResult
        {
            private AndroidFacebook androidImpl;

            public JavaMethodCall(AndroidFacebook androidImpl, string methodName)
                : base(androidImpl, methodName)
            {
                this.androidImpl = androidImpl;
            }

            public override void Call(MethodArguments args = null)
            {
                MethodArguments paramsCopy;
                if (args == null)
                {
                    paramsCopy = new MethodArguments();
                }
                else
                {
                    paramsCopy = new MethodArguments(args);
                }

                if (this.Callback != null)
                {
                    paramsCopy.AddString("callback_id", this.androidImpl.CallbackManager.AddFacebookDelegate(this.Callback));
                }

                this.androidImpl.CallFB(this.MethodName, paramsCopy.ToJsonString());
            }
        }
    }
}
