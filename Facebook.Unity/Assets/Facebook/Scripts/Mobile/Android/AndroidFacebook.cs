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

    internal sealed class AndroidFacebook : MobileFacebook
    {
        // This class holds all the of the wrapper methods that we call into
        private bool limitEventUsage;
        private IAndroidJavaClass facebookJava;

        public AndroidFacebook() : this(new FBJavaClass(), new CallbackManager())
        {
        }

        public AndroidFacebook(IAndroidJavaClass facebookJavaClass, CallbackManager callbackManager)
            : base(callbackManager)
        {
            this.KeyHash = string.Empty;
            this.facebookJava = facebookJavaClass;
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

        public override string FacebookSdkVersion
        {
            get
            {
                string buildVersion = this.facebookJava.CallStatic<string>("GetSdkVersion");
                return string.Format("Facebook.Android.SDK.{0}", buildVersion);
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

            var args = new MethodArguments();
            args.AddString("appId", appId);
            args.AddPrimative("cookie", cookie);
            args.AddPrimative("logging", logging);
            args.AddPrimative("status", status);
            args.AddPrimative("xfbml", xfbml);
            args.AddString("channelUrl", channelUrl);
            args.AddString("authResponse", authResponse);
            args.AddPrimative("frictionlessRequests", frictionlessRequests);
            var initCall = new JavaMethodCall<IResult>(this, "Init");
            initCall.Call(args);
            this.CallFB(
                "SetUserAgentSuffix",
                string.Format("Unity.{0}", Facebook.Unity.FacebookSdkVersion.Build));
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddCommaSeparatedList("scope", permissions);
            var loginCall = new JavaMethodCall<ILoginResult>(this, "LoginWithReadPermissions");
            loginCall.Callback = callback;
            loginCall.Call(args);
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddCommaSeparatedList("scope", permissions);
            var loginCall = new JavaMethodCall<ILoginResult>(this, "LoginWithPublishPermissions");
            loginCall.Callback = callback;
            loginCall.Call(args);
        }

        public override void LogOut()
        {
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

        public override void AppInvite(
            Uri appLinkUrl,
            Uri previewImageUrl,
            FacebookDelegate<IAppInviteResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddUri("appLinkUrl", appLinkUrl);
            args.AddUri("previewImageUrl", previewImageUrl);
            var appInviteCall = new JavaMethodCall<IAppInviteResult>(this, "AppInvite");
            appInviteCall.Callback = callback;
            appInviteCall.Call(args);
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

        public override void GameGroupCreate(
            string name,
            string description,
            string privacy,
            FacebookDelegate<IGroupCreateResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("name", name);
            args.AddString("description", description);
            args.AddString("privacy", privacy);
            var gameGroupCreate = new JavaMethodCall<IGroupCreateResult>(this, "GameGroupCreate");
            gameGroupCreate.Callback = callback;
            gameGroupCreate.Call(args);
        }

        public override void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("id", id);
            var groupJoinCall = new JavaMethodCall<IGroupJoinResult>(this, "GameGroupJoin");
            groupJoinCall.Callback = callback;
            groupJoinCall.Call(args);
        }

        public override void GetAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            var getAppLink = new JavaMethodCall<IAppLinkResult>(this, "GetAppLink");
            getAppLink.Callback = callback;
            getAppLink.Call();
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("logEvent", logEvent);
            args.AddNullablePrimitive("valueToSum", valueToSum);
            args.AddDictionary("parameters", parameters);
            var appEventcall = new JavaMethodCall<IResult>(this, "AppEvents");
            appEventcall.Call(args);
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            MethodArguments args = new MethodArguments();
            args.AddPrimative("logPurchase", logPurchase);
            args.AddString("currency", currency);
            args.AddDictionary("parameters", parameters);
            var logPurchaseCall = new JavaMethodCall<IResult>(this, "AppEvents");
            logPurchaseCall.Call(args);
        }

        public override void ActivateApp(string appId)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("app_id", appId);
            var activateApp = new JavaMethodCall<IResult>(this, "ActivateApp");
            activateApp.Call(args);
        }

        public override void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            MethodArguments args = new MethodArguments();
            var fetchDeferredAppLinkData = new JavaMethodCall<IAppLinkResult>(this, "FetchDeferredAppLinkData");
            fetchDeferredAppLinkData.Callback = callback;
            fetchDeferredAppLinkData.Call(args);
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            this.CallFB("SetShareDialogMode", mode.ToString());
        }

        private void CallFB(string method, string args)
        {
            this.facebookJava.CallStatic(method, args);
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
