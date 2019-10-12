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

        public override void SetAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled)
        {
            this.CallFB("SetAutoLogAppEventsEnabled", autoLogAppEventsEnabled.ToString());
        }

        public override void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled)
        {
            this.CallFB("SetAdvertiserIDCollectionEnabled", advertiserIDCollectionEnabled.ToString());
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
