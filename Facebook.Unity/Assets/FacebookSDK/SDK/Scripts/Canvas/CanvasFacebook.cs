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

namespace Facebook.Unity.Canvas
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    internal sealed class CanvasFacebook : FacebookBase, ICanvasFacebookImplementation
    {
        internal const string MethodAppRequests = "apprequests";
        internal const string MethodFeed = "feed";
        internal const string MethodPay = "pay";
        internal const string MethodGameGroupCreate = "game_group_create";
        internal const string MethodGameGroupJoin = "game_group_join";
        internal const string CancelledResponse = "{\"cancelled\":true}";
        internal const string FacebookConnectURL = "https://connect.facebook.net";

        private const string AuthResponseKey = "authResponse";
        private const string ResponseKey = "response";

        private string appId;
        private string appLinkUrl;
        private ICanvasJSWrapper canvasJSWrapper;

        public CanvasFacebook()
            : this(new CanvasJSWrapper(), new CallbackManager())
        {
        }

        public CanvasFacebook(ICanvasJSWrapper canvasJSWrapper, CallbackManager callbackManager)
            : base(callbackManager)
        {
            this.canvasJSWrapper = canvasJSWrapper;
        }

        public override bool LimitEventUsage { get; set; }

        public override string SDKName
        {
            get
            {
                return "FBJSSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return this.canvasJSWrapper.GetSDKVersion();
            }
        }

        public override string SDKUserAgent
        {
            get
            {
                // We want to log whether we are running as webgl or in the web player.
                string webPlatform;

                switch (Constants.CurrentPlatform)
                {
                    case FacebookUnityPlatform.WebGL:
                    case FacebookUnityPlatform.WebPlayer:
                        webPlatform = string.Format(
                            CultureInfo.InvariantCulture,
                            "FBUnity{0}",
                            Constants.CurrentPlatform.ToString());
                        break;
                    default:
                        FacebookLogger.Warn("Currently running on uknown web platform");
                        webPlatform = "FBUnityWebUnknown";
                        break;
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    base.SDKUserAgent,
                    Utilities.GetUserAgent(webPlatform, FacebookSdkVersion.Build));
            }
        }

        public void Init(
                string appId,
                bool cookie,
                bool logging,
                bool status,
                bool xfbml,
                string channelUrl,
                string authResponse,
                bool frictionlessRequests,
                string jsSDKLocale,
                HideUnityDelegate hideUnityDelegate,
                InitDelegate onInitComplete)
        {
            if (this.canvasJSWrapper.IntegrationMethodJs == null)
            {
                throw new Exception("Cannot initialize facebook javascript");
            }

            base.Init(
                hideUnityDelegate,
                onInitComplete);

            this.canvasJSWrapper.ExternalEval(this.canvasJSWrapper.IntegrationMethodJs);
            this.appId = appId;

            bool isPlayer = true;
            #if UNITY_WEBGL
            isPlayer = false;
            #endif

            MethodArguments parameters = new MethodArguments();
            parameters.AddString("appId", appId);
            parameters.AddPrimative("cookie", cookie);
            parameters.AddPrimative("logging", logging);
            parameters.AddPrimative("status", status);
            parameters.AddPrimative("xfbml", xfbml);
            parameters.AddString("channelUrl", channelUrl);
            parameters.AddString("authResponse", authResponse);
            parameters.AddPrimative("frictionlessRequests", frictionlessRequests);
            parameters.AddString("version", Constants.GraphAPIVersion);

            // use 1/0 for booleans, otherwise you'll get strings "True"/"False"
            this.canvasJSWrapper.ExternalCall(
                "FBUnity.init",
                isPlayer ? 1 : 0,
                FacebookConnectURL,
                jsSDKLocale,
                Constants.DebugMode ? 1 : 0,
                parameters.ToJsonString(),
                status ? 1 : 0);
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.canvasJSWrapper.DisableFullScreen();
            this.canvasJSWrapper.ExternalCall("FBUnity.login", permissions, CallbackManager.AddFacebookDelegate(callback));
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.canvasJSWrapper.DisableFullScreen();
            this.canvasJSWrapper.ExternalCall("FBUnity.login", permissions, CallbackManager.AddFacebookDelegate(callback));
        }

        public override void LogOut()
        {
            base.LogOut();
            this.canvasJSWrapper.ExternalCall("FBUnity.logout");
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
            args.AddCommaSeparatedList("to", to);
            args.AddString("action_type", actionType != null ? actionType.ToString() : null);
            args.AddString("object_id", objectId);
            args.AddList("filters", filters);
            args.AddList("exclude_ids", excludeIds);
            args.AddNullablePrimitive("max_recipients", maxRecipients);
            args.AddString("data", data);
            args.AddString("title", title);
            var call = new CanvasUIMethodCall<IAppRequestResult>(this, MethodAppRequests, Constants.OnAppRequestsCompleteMethodName);
            call.Callback = callback;
            call.Call(args);
        }

        public override void ActivateApp(string appId)
        {
            this.canvasJSWrapper.ExternalCall("FBUnity.activateApp");
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddUri("link", contentURL);
            args.AddString("name", contentTitle);
            args.AddString("description", contentDescription);
            args.AddUri("picture", photoURL);
            var call = new CanvasUIMethodCall<IShareResult>(this, MethodFeed, Constants.OnShareCompleteMethodName);
            call.Callback = callback;
            call.Call(args);
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
            args.AddString("to", toId);
            args.AddUri("link", link);
            args.AddString("name", linkName);
            args.AddString("caption", linkCaption);
            args.AddString("description", linkDescription);
            args.AddUri("picture", picture);
            args.AddString("source", mediaSource);
            var call = new CanvasUIMethodCall<IShareResult>(this, MethodFeed, Constants.OnShareCompleteMethodName);
            call.Callback = callback;
            call.Call(args);
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
            MethodArguments args = new MethodArguments();
            args.AddString("product", product);
            args.AddString("action", action);
            args.AddPrimative("quantity", quantity);
            args.AddNullablePrimitive("quantity_min", quantityMin);
            args.AddNullablePrimitive("quantity_max", quantityMax);
            args.AddString("request_id", requestId);
            args.AddString("pricepoint_id", pricepointId);
            args.AddString("test_currency", testCurrency);
            var call = new CanvasUIMethodCall<IPayResult>(this, MethodPay, Constants.OnPayCompleteMethodName);
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
            args.AddString("display", "async");
            var call = new CanvasUIMethodCall<IGroupCreateResult>(this, MethodGameGroupCreate, Constants.OnGroupCreateCompleteMethodName);
            call.Callback = callback;
            call.Call(args);
        }

        public override void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("id", id);
            args.AddString("display", "async");
            var call = new CanvasUIMethodCall<IGroupJoinResult>(this, MethodGameGroupJoin, Constants.OnGroupJoinCompleteMethodName);
            call.Callback = callback;
            call.Call(args);
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            var result = new Dictionary<string, object>()
            {
                {
                    "url", this.appLinkUrl
                }
            };
            callback(new AppLinkResult(MiniJSON.Json.Serialize(result)));
            this.appLinkUrl = string.Empty;
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            this.canvasJSWrapper.ExternalCall(
                "FBUnity.logAppEvent",
                logEvent,
                valueToSum,
                MiniJSON.Json.Serialize(parameters));
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            this.canvasJSWrapper.ExternalCall(
                "FBUnity.logPurchase",
                logPurchase,
                currency,
                MiniJSON.Json.Serialize(parameters));
        }

        public override void OnLoginComplete(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatAuthResponse(responseJsonData);
            this.OnAuthResponse(new LoginResult(formattedResponse));
        }

        public override void OnGetAppLinkComplete(string message)
        {
            // We should never get here on canvas. We store the app link on this object
            // so should never hit this method.
            throw new NotImplementedException();
        }

        // used only to refresh the access token
        public void OnFacebookAuthResponseChange(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatAuthResponse(responseJsonData);
            var result = new LoginResult(formattedResponse);
            AccessToken.CurrentAccessToken = result.AccessToken;
        }

        public void OnPayComplete(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatResult(responseJsonData);
            var result = new PayResult(formattedResponse);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnAppRequestsComplete(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatResult(responseJsonData);
            var result = new AppRequestResult(formattedResponse);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatResult(responseJsonData);
            var result = new ShareResult(formattedResponse);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupCreateComplete(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatResult(responseJsonData);
            var result = new GroupCreateResult(formattedResponse);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupJoinComplete(string responseJsonData)
        {
            string formattedResponse = CanvasFacebook.FormatResult(responseJsonData);
            var result = new GroupJoinResult(formattedResponse);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnUrlResponse(string url)
        {
            this.appLinkUrl = url;
        }

        private static string FormatAuthResponse(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            IDictionary<string, object> responseDictionary = GetFormattedResponseDictionary(result);
            IDictionary<string, object> authResponse;
            if (responseDictionary.TryGetValue(CanvasFacebook.AuthResponseKey, out authResponse))
            {
                responseDictionary.Remove(CanvasFacebook.AuthResponseKey);
                foreach (var item in authResponse)
                {
                    responseDictionary[item.Key] = item.Value;
                }
            }

            return MiniJSON.Json.Serialize(responseDictionary);
        }

        // This method converts the format of the result to match the format
        // of our results from iOS and Android
        private static string FormatResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            return MiniJSON.Json.Serialize(GetFormattedResponseDictionary(result));
        }

        private static IDictionary<string, object> GetFormattedResponseDictionary(string result)
        {
            var resultDictionary = (IDictionary<string, object>)MiniJSON.Json.Deserialize(result);
            IDictionary<string, object> responseDictionary;
            if (resultDictionary.TryGetValue(CanvasFacebook.ResponseKey, out responseDictionary))
            {
                object callbackId;
                if (resultDictionary.TryGetValue(Constants.CallbackIdKey, out callbackId))
                {
                    responseDictionary[Constants.CallbackIdKey] = callbackId;
                }

                return responseDictionary;
            }

            return resultDictionary;
        }

        private class CanvasUIMethodCall<T> : MethodCall<T> where T : IResult
        {
            private CanvasFacebook canvasImpl;
            private string callbackMethod;

            public CanvasUIMethodCall(CanvasFacebook canvasImpl, string methodName, string callbackMethod)
                : base(canvasImpl, methodName)
            {
                this.canvasImpl = canvasImpl;
                this.callbackMethod = callbackMethod;
            }

            public override void Call(MethodArguments args)
            {
                this.UI(this.MethodName, args, this.Callback);
            }

            private void UI(
                string method,
                MethodArguments args,
                FacebookDelegate<T> callback = null)
            {
                this.canvasImpl.canvasJSWrapper.DisableFullScreen();

                var clonedArgs = new MethodArguments(args);
                clonedArgs.AddString("app_id", this.canvasImpl.appId);
                clonedArgs.AddString("method", method);
                var uniqueId = this.canvasImpl.CallbackManager.AddFacebookDelegate(callback);
                this.canvasImpl.canvasJSWrapper.ExternalCall("FBUnity.ui", clonedArgs.ToJsonString(), uniqueId, this.callbackMethod);
            }
        }
    }
}
