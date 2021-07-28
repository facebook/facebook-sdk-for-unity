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

namespace Facebook.Unity.Mobile.IOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class IOSFacebook : MobileFacebook
    {
        private const string CancelledResponse = "{\"cancelled\":true}";
        private bool limitEventUsage;
        private IIOSWrapper iosWrapper;
        private string userID;

        public IOSFacebook()
            : this(GetIOSWrapper(), new CallbackManager())
        {
        }

        public IOSFacebook(IIOSWrapper iosWrapper, CallbackManager callbackManager)
            : base(callbackManager)
        {
            this.iosWrapper = iosWrapper;
        }

        public enum FBInsightsFlushBehavior
        {
            /// <summary>
            /// The FB insights flush behavior auto.
            /// </summary>
            FBInsightsFlushBehaviorAuto,

            /// <summary>
            /// The FB insights flush behavior explicit only.
            /// </summary>
            FBInsightsFlushBehaviorExplicitOnly,
        }

        public override bool LimitEventUsage
        {
            get
            {
                return this.limitEventUsage;
            }

            set
            {
                this.limitEventUsage = value;
                this.iosWrapper.FBAppEventsSetLimitEventUsage(value);
            }
        }

        public override void SetAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled)
        {
            this.iosWrapper.FBAutoLogAppEventsEnabled(autoLogAppEventsEnabled);
        }

        public override void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled)
        {
            this.iosWrapper.FBAdvertiserIDCollectionEnabled(advertiserIDCollectionEnabled);
        }

        public override bool SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            return this.iosWrapper.FBAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }

        public override void SetPushNotificationsDeviceTokenString(string token)
        {
            this.iosWrapper.SetPushNotificationsDeviceTokenString(token);
        }

        public override string SDKName
        {
            get
            {
                return "FBiOSSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return this.iosWrapper.FBSdkVersion();
            }
        }

        public override string UserID
        {
            get
            {
                return this.userID;
            }

            set
            {
                this.userID = value;
                this.iosWrapper.FBSetUserID(value);
            }
        }

        public override void UpdateUserProperties(Dictionary<string, string> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            this.iosWrapper.UpdateUserProperties(dict.NumEntries, dict.Keys, dict.Values);
        }

        public override void SetDataProcessingOptions(IEnumerable<string> options, int country, int state)
        {
            this.iosWrapper.SetDataProcessingOptions(options.ToArray(), country, state);
        }

        public void Init(
            string appId,
            bool frictionlessRequests,
            string iosURLSuffix,
            HideUnityDelegate hideUnityDelegate,
            InitDelegate onInitComplete)
        {
            base.Init(onInitComplete);

            this.iosWrapper.Init(
                appId,
                frictionlessRequests,
                iosURLSuffix,
                Constants.UnitySDKUserAgentSuffixLegacy);
            this.userID = this.iosWrapper.FBGetUserID();
        }

        public override void EnableProfileUpdatesOnAccessTokenChange(bool enable)
        {
            this.iosWrapper.EnableProfileUpdatesOnAccessTokenChange(enable);
        }

        public override void LoginWithTrackingPreference(
            string tracking,
            IEnumerable<string> permissions,
            string nonce,
            FacebookDelegate<ILoginResult> callback)
        {
            this.iosWrapper.LoginWithTrackingPreference(this.AddCallback(callback), permissions.ToCommaSeparateList(), tracking, nonce);
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.iosWrapper.LogInWithReadPermissions(this.AddCallback(callback), permissions.ToCommaSeparateList());
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.iosWrapper.LogInWithPublishPermissions(this.AddCallback(callback), permissions.ToCommaSeparateList());
        }

        public override void LogOut()
        {
            base.LogOut();
            this.iosWrapper.LogOut();
        }

        public override bool LoggedIn
        {
            get
            {
                AccessToken token = AccessToken.CurrentAccessToken;
                AuthenticationToken authenticationToken = CurrentAuthenticationToken();
                return (token != null && token.ExpirationTime > DateTime.UtcNow) || authenticationToken != null;
            }
        }

        public override AuthenticationToken CurrentAuthenticationToken()
        {
            return this.iosWrapper.CurrentAuthenticationToken();
        }

        public override Profile CurrentProfile()
        {
            return this.iosWrapper.CurrentProfile();
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

            string mobileFilter = null;
            if (filters != null && filters.Any())
            {
                mobileFilter = filters.First() as string;
            }

            this.iosWrapper.AppRequest(
                this.AddCallback(callback),
                message,
                (actionType != null) ? actionType.ToString() : string.Empty,
                objectId != null ? objectId : string.Empty,
                to != null ? to.ToArray() : null,
                to != null ? to.Count() : 0,
                mobileFilter != null ? mobileFilter : string.Empty,
                excludeIds != null ? excludeIds.ToArray() : null,
                excludeIds != null ? excludeIds.Count() : 0,
                maxRecipients.HasValue,
                maxRecipients.HasValue ? maxRecipients.Value : 0,
                data,
                title);
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            this.iosWrapper.ShareLink(
                this.AddCallback(callback),
                contentURL.AbsoluteUrlOrEmptyString(),
                contentTitle,
                contentDescription,
                photoURL.AbsoluteUrlOrEmptyString());
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
            string linkStr = link != null ? link.ToString() : string.Empty;
            string pictureStr = picture != null ? picture.ToString() : string.Empty;
            this.iosWrapper.FeedShare(
                this.AddCallback(callback),
                toId,
                linkStr,
                linkName,
                linkCaption,
                linkDescription,
                pictureStr,
                mediaSource);
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            if (valueToSum.HasValue)
            {
                this.iosWrapper.LogAppEvent(logEvent, valueToSum.Value, dict.NumEntries, dict.Keys, dict.Values);
            }
            else
            {
                this.iosWrapper.LogAppEvent(logEvent, 0.0, dict.NumEntries, dict.Keys, dict.Values);
            }
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            this.iosWrapper.LogPurchaseAppEvent(logPurchase, currency, dict.NumEntries, dict.Keys, dict.Values);
        }

        public override bool IsImplicitPurchaseLoggingEnabled()
        {
            return false;
        }

        public override void ActivateApp(string appId)
        {
            this.iosWrapper.FBAppEventsActivateApp();
        }

        public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            this.iosWrapper.FetchDeferredAppLink(this.AddCallback(callback));
        }

        public override void GetAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            this.iosWrapper.GetAppLink(System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)));
        }

        public override void OpenFriendFinderDialog(
            FacebookDelegate<IGamingServicesFriendFinderResult> callback)
		{
            this.iosWrapper.OpenFriendFinderDialog(System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)));
		}

        public override void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback)
        {
            this.iosWrapper.RefreshCurrentAccessToken(
                System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)));
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            this.iosWrapper.SetShareDialogMode((int)mode);
        }

        public override void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            this.iosWrapper.UploadImageToMediaLibrary(
                System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)),
                caption,
                imageUri.AbsolutePath.ToString(),
                shouldLaunchMediaDialog);
        }

        public override void UploadVideoToMediaLibrary(
            string caption,
            Uri videoUri,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            this.iosWrapper.UploadVideoToMediaLibrary(
                System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)),
                caption,
                videoUri.AbsolutePath.ToString());
        }

        private static IIOSWrapper GetIOSWrapper()
        {
            Assembly assembly = Assembly.Load("Facebook.Unity.IOS");
            Type type = assembly.GetType("Facebook.Unity.IOS.IOSWrapper");
            IIOSWrapper iosWrapper = (IIOSWrapper)Activator.CreateInstance(type);
            return iosWrapper;
        }

        private static NativeDict MarshallDict(Dictionary<string, object> dict)
        {
            NativeDict res = new NativeDict();

            if (dict != null && dict.Count > 0)
            {
                res.Keys = new string[dict.Count];
                res.Values = new string[dict.Count];
                res.NumEntries = 0;
                foreach (KeyValuePair<string, object> kvp in dict)
                {
                    res.Keys[res.NumEntries] = kvp.Key;
                    res.Values[res.NumEntries] = kvp.Value.ToString();
                    res.NumEntries++;
                }
            }

            return res;
        }

        private static NativeDict MarshallDict(Dictionary<string, string> dict)
        {
            NativeDict res = new NativeDict();

            if (dict != null && dict.Count > 0)
            {
                res.Keys = new string[dict.Count];
                res.Values = new string[dict.Count];
                res.NumEntries = 0;
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    res.Keys[res.NumEntries] = kvp.Key;
                    res.Values[res.NumEntries] = kvp.Value;
                    res.NumEntries++;
                }
            }

            return res;
        }

        private int AddCallback<T>(FacebookDelegate<T> callback) where T : IResult
        {
            string asyncId = this.CallbackManager.AddFacebookDelegate(callback);
            return Convert.ToInt32(asyncId);
        }

        private class NativeDict
        {
            public NativeDict()
            {
                this.NumEntries = 0;
                this.Keys = null;
                this.Values = null;
            }

            public int NumEntries { get; set; }

            public string[] Keys { get; set; }

            public string[] Values { get; set; }
        }
    }
}
