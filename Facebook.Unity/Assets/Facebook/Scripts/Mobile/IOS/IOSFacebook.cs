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
    using System.Runtime.InteropServices;

    internal class IOSFacebook : MobileFacebook
    {
        private const string CancelledResponse = "{\"cancelled\":true}";
        private bool limitEventUsage;

        public IOSFacebook()
            : this(new CallbackManager())
        {
        }

        public IOSFacebook(CallbackManager callbackManager)
            : base(callbackManager)
        {
        }

        public enum FBInsightsFlushBehavior
        {
            FBInsightsFlushBehaviorAuto,
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
                IOSFacebook.IOSFBAppEventsSetLimitEventUsage(value);
            }
        }

        public override string FacebookSdkVersion
        {
            get
            {
                return string.Format("Facebook.iOS.SDK.{0}", IOSFacebook.IOSFBSdkVersion());
            }
        }

        public override void Init(
            string appId,
            bool cookie,
            bool logging,
            bool status,
            bool xfbml,
            string channelUrl,
            string authResponse,
            bool frictionlessRequests,
            HideUnityDelegate hideUnityDelegate,
            InitDelegate onInitComplete)
        {
            base.Init(
                appId,
                cookie,
                logging,
                status,
                xfbml,
                channelUrl,
                authResponse,
                frictionlessRequests,
                hideUnityDelegate,
                onInitComplete);

            string unityUserAgentSuffix = string.Format(
                "Unity.{0}",
                Facebook.Unity.FacebookSdkVersion.Build);

            IOSFacebook.IOSInit(
                appId,
                cookie,
                logging,
                status,
                frictionlessRequests,
                FacebookSettings.IosURLSuffix,
                unityUserAgentSuffix);
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            IOSFacebook.IOSLogInWithReadPermissions(this.AddCallback(callback), permissions.ToCommaSeparateList());
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            IOSFacebook.IOSLogInWithPublishPermissions(this.AddCallback(callback), permissions.ToCommaSeparateList());
        }

        public override void LogOut()
        {
            base.LogOut();
            IOSFacebook.IOSLogOut();
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

            IOSFacebook.IOSAppRequest(
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

        public override void AppInvite(
            Uri appLinkUrl,
            Uri previewImageUrl,
            FacebookDelegate<IAppInviteResult> callback)
        {
            string appLinkUrlStr = string.Empty;
            string previewImageUrlStr = string.Empty;
            if (appLinkUrl != null && !string.IsNullOrEmpty(appLinkUrl.AbsoluteUri))
            {
                appLinkUrlStr = appLinkUrl.AbsoluteUri;
            }

            if (previewImageUrl != null && !string.IsNullOrEmpty(previewImageUrl.AbsoluteUri))
            {
                previewImageUrlStr = previewImageUrl.AbsoluteUri;
            }

            IOSFacebook.IOSAppInvite(
                this.AddCallback(callback),
                appLinkUrlStr,
                previewImageUrlStr);
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            IOSFacebook.IOSShareLink(
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
            IOSFacebook.IOSFeedShare(
                this.AddCallback(callback),
                toId,
                linkStr,
                linkName,
                linkCaption,
                linkDescription,
                pictureStr,
                mediaSource);
        }

        public override void GameGroupCreate(
            string name,
            string description,
            string privacy,
            FacebookDelegate<IGroupCreateResult> callback)
        {
            IOSFacebook.IOSCreateGameGroup(this.AddCallback(callback), name, description, privacy);
        }

        public override void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback)
        {
            IOSFacebook.IOSJoinGameGroup(System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)), id);
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            if (valueToSum.HasValue)
            {
                IOSFacebook.IOSFBAppEventsLogEvent(logEvent, valueToSum.Value, dict.NumEntries, dict.Keys, dict.Values);
            }
            else
            {
                IOSFacebook.IOSFBAppEventsLogEvent(logEvent, 0.0, dict.NumEntries, dict.Keys, dict.Values);
            }
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            IOSFacebook.IOSFBAppEventsLogPurchase(logPurchase, currency, dict.NumEntries, dict.Keys, dict.Values);
        }

        public override void ActivateApp(string appId)
        {
            IOSFacebook.IOSFBSettingsActivateApp(appId);
        }

        public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            IOSFacebook.IOSFetchDeferredAppLink(this.AddCallback(callback));
        }

        public override void GetAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            IOSFacebook.IOSGetAppLink(System.Convert.ToInt32(CallbackManager.AddFacebookDelegate(callback)));
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            IOSFacebook.IOSSetShareDialogMode((int)mode);
        }

        #if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern void IOSInit(
        string appId,
        bool cookie,
        bool logging,
        bool status,
        bool frictionlessRequests,
        string urlSuffix,
        string unityUserAgentSuffix);

        [DllImport ("__Internal")]
        private static extern void IOSLogInWithReadPermissions(
        int requestId,
        string scope);

        [DllImport ("__Internal")]
        private static extern void IOSLogInWithPublishPermissions(
        int requestId,
        string scope);

        [DllImport ("__Internal")]
        private static extern void IOSLogOut();

        [DllImport ("__Internal")]
        private static extern void IOSSetShareDialogMode(int mode);

        [DllImport ("__Internal")]
        private static extern void IOSShareLink(
        int requestId,
        string contentURL,
        string contentTitle,
        string contentDescription,
        string photoURL);

        [DllImport ("__Internal")]
        public static extern void IOSFeedShare(
        int requestId,
        string toId,
        string link,
        string linkName,
        string linkCaption,
        string linkDescription,
        string picture,
        string mediaSource);

        [DllImport ("__Internal")]
        private static extern void IOSAppRequest(
        int requestId,
        string message,
        string actionType,
        string objectId,
        string[] to = null,
        int toLength = 0,
        string filters = "",
        string[] excludeIds = null,
        int excludeIdsLength = 0,
        bool hasMaxRecipients = false,
        int maxRecipients = 0,
        string data = "",
        string title = "");

        [DllImport ("__Internal")]
        private static extern void IOSAppInvite(
        int requestId,
        string appLinkUrl,
        string previewImageUrl);

        [DllImport ("__Internal")]
        private static extern void IOSCreateGameGroup(
        int requestId,
        string name,
        string description,
        string privacy);

        [DllImport ("__Internal")]
        private static extern void IOSJoinGameGroup(int requestId, string groupId);

        [DllImport ("__Internal")]
        private static extern void IOSFBSettingsActivateApp(string appId);

        [DllImport ("__Internal")]
        private static extern void IOSFBAppEventsLogEvent(
        string logEvent,
        double valueToSum,
        int numParams,
        string[] paramKeys,
        string[] paramVals);

        [DllImport ("__Internal")]
        private static extern void IOSFBAppEventsLogPurchase(
        double logPurchase,
        string currency,
        int numParams,
        string[] paramKeys,
        string[] paramVals);

        [DllImport ("__Internal")]
        private static extern void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage);

        [DllImport ("__Internal")]
        private static extern void IOSGetAppLink(int requestID);

        [DllImport ("__Internal")]
        private static extern string IOSFBSdkVersion();

        [DllImport ("__Internal")]
        private static extern void IOSFetchDeferredAppLink(int requestID);
        #else
        private static void IOSInit(
            string appId,
            bool cookie,
            bool logging,
            bool status,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix)
        {
        }

        private static void IOSLogInWithReadPermissions(
            int requestId,
            string scope)
        {
        }

        private static void IOSLogInWithPublishPermissions(
            int requestId,
            string scope)
        {
        }

        private static void IOSLogOut()
        {
        }

        private static void IOSSetShareDialogMode(int mode)
        {
        }

        private static void IOSShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL)
        {
        }

        private static void IOSFeedShare(
            int requestId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string picture,
            string mediaSource)
        {
        }

        private static void IOSAppRequest(
            int requestId,
            string message,
            string actionType,
            string objectId,
            string[] to = null,
            int toLength = 0,
            string filters = "",
            string[] excludeIds = null,
            int excludeIdsLength = 0,
            bool hasMaxRecipients = false,
            int maxRecipients = 0,
            string data = "",
            string title = "")
        {
        }

        private static void IOSAppInvite(
            int requestId,
            string appLinkUrl,
            string previewImageUrl)
        {
        }

        private static void IOSCreateGameGroup(
            int requestId,
            string name,
            string description,
            string privacy)
        {
        }

        private static void IOSJoinGameGroup(int requestId, string groupId)
        {
        }

        private static void IOSFBSettingsPublishInstall(int requestId, string appId)
        {
        }

        private static void IOSFBSettingsActivateApp(string appId)
        {
        }

        private static void IOSFBAppEventsLogEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
        }

        private static void IOSFBAppEventsLogPurchase(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
        }

        private static void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage)
        {
        }

        private static void IOSGetAppLink(int requestId)
        {
        }

        private static string IOSFBSdkVersion()
        {
            return "NONE";
        }

        private static void IOSFetchDeferredAppLink(int requestId)
        {
        }
        #endif

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
