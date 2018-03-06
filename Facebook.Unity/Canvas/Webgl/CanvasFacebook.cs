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

namespace Facebook.Unity.Canvas.Webgl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal sealed class CanvasFacebook : FacebookBase, ICanvasFacebookImplementation
    {
        internal const string MethodAppRequests = "apprequests";
        internal const string MethodFeed = "feed";
        internal const string MethodPay = "pay";
        internal const string CancelledResponse = "{\"cancelled\":true}";
        internal const string FacebookConnectURL = "https://connect.facebook.net";

        private const string AuthResponseKey = "authResponse";

        private string appId;
        private string appLinkUrl;
        private ICanvasWrapper wrapper;
        private ICanvasJSWrapper canvasJSWrapper;
        private HideUnityDelegate onHideUnityDelegate;



        public CanvasFacebook()
            : this(GetCanvasWrapper(), new CanvasJSWrapper(), new CallbackManager())
        {
        }

        public CanvasFacebook(ICanvasWrapper wrapper, ICanvasJSWrapper canvasJSWrapper, CallbackManager callbackManager)
            : base(callbackManager)
        {
            this.wrapper = wrapper;
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
                string javascriptSDKLocale,
                bool loadDebugJSSDK,
                HideUnityDelegate hideUnityDelegate,
                InitDelegate onInitComplete)
        {
            base.Init(onInitComplete);

            wrapper.InitScreenPosition();
            this.appId = appId;
            this.onHideUnityDelegate = hideUnityDelegate;

            MethodArguments parameters = new MethodArguments();
            parameters.AddString("appId", appId);
            parameters.AddPrimative("cookie", cookie);
            parameters.AddPrimative("logging", logging);
            parameters.AddPrimative("status", status);
            parameters.AddPrimative("xfbml", xfbml);
            parameters.AddString("channelUrl", channelUrl);
            parameters.AddString("authResponse", authResponse);
            parameters.AddPrimative("frictionlessRequests", frictionlessRequests);
            parameters.AddString("version", FB.GraphApiVersion);

            // use 1/0 for booleans, otherwise you'll get strings "True"/"False"
            wrapper.Init(
              FacebookConnectURL,
              javascriptSDKLocale,
              loadDebugJSSDK ? 1 : 0,
              parameters.ToJsonString(),
              status ? 1 : 0);
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.canvasJSWrapper.DisableFullScreen();
            wrapper.Login(permissions.ToCommaSeparateList(), CallbackManager.AddFacebookDelegate(callback));
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            this.canvasJSWrapper.DisableFullScreen();
            wrapper.Login(permissions.ToCommaSeparateList(), CallbackManager.AddFacebookDelegate(callback));
        }

        public override void LogOut()
        {
            base.LogOut();
            wrapper.LogOut();
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
            wrapper.ActivateApp();
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
            this.PayImpl(
                 product,
                 /*productId*/ null,
                 action,
                 quantity,
                 quantityMin,
                 quantityMax,
                 requestId,
                 pricepointId,
                 testCurrency,
                 /*developerPayload*/ null,
                 callback);
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
            this.PayImpl(
                /*product*/ null,
                productId,
                action,
                quantity,
                quantityMin,
                quantityMax,
                requestId,
                pricepointId,
                testCurrency,
                /*developerPayload*/ null,
                callback);
        }

        public void PayWithProductId(
            string productId,
            string action,
            string developerPayload,
            string testCurrency,
            FacebookDelegate<IPayResult> callback)
        {
            this.PayImpl(
                /*product*/ null,
                productId,
                action,
                /*quantity*/ 1,
                /*quantityMin*/ null,
                /*quantityMax*/ null,
                /*requestId*/ null,
                /*pricepointId*/ null,
                testCurrency,
                developerPayload,
                callback);
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            var result = new Dictionary<string, object>()
            {
                {
                    "url", this.appLinkUrl
                }
            };
            callback(new AppLinkResult(new ResultContainer(result)));
            this.appLinkUrl = string.Empty;
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            wrapper.LogAppEvent(
                logEvent,
                valueToSum,
                MiniJSON.Json.Serialize(parameters));
        }

        public override void AppEventsLogPurchase(
            float purchaseAmount,
            string currency,
            Dictionary<string, object> parameters)
        {
            wrapper.LogPurchase(
                purchaseAmount,
                currency,
                MiniJSON.Json.Serialize(parameters));
        }

        public override void OnLoginComplete(ResultContainer result)
        {
            CanvasFacebook.FormatAuthResponse(
                result,
                (formattedResponse) =>
                    {
                        this.OnAuthResponse(new LoginResult(formattedResponse));
                    });
        }

        public override void OnGetAppLinkComplete(ResultContainer message)
        {
            // We should never get here on canvas. We store the app link on this object
            // so should never hit this method.
            throw new NotImplementedException();
        }

        // used only to refresh the access token
        public void OnFacebookAuthResponseChange(string responseJsonData)
        {
            this.OnFacebookAuthResponseChange(new ResultContainer(responseJsonData));
        }

        public void OnFacebookAuthResponseChange(ResultContainer resultContainer)
        {
            CanvasFacebook.FormatAuthResponse(
                resultContainer,
                (formattedResponse) =>
                {
                    var result = new LoginResult(formattedResponse);
                    AccessToken.CurrentAccessToken = result.AccessToken;
                });
        }

        public void OnPayComplete(string responseJsonData)
        {
            this.OnPayComplete(new ResultContainer(responseJsonData));
        }

        public void OnPayComplete(ResultContainer resultContainer)
        {
            var result = new PayResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnAppRequestsComplete(ResultContainer resultContainer)
        {
            var result = new AppRequestResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(ResultContainer resultContainer)
        {
            var result = new ShareResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnUrlResponse(string url)
        {
            this.appLinkUrl = url;
        }

        public void OnHideUnity(bool isGameShown)
        {
            if (this.onHideUnityDelegate != null)
            {
                this.onHideUnityDelegate(isGameShown);
            }
        }

        private static ICanvasWrapper GetCanvasWrapper()
        {
            Assembly assembly = Assembly.Load("Facebook.Unity.Canvas");
            Type type = assembly.GetType("Facebook.Unity.Canvas.CanvasWrapper");
            ICanvasWrapper canvasWrapper = (ICanvasWrapper)Activator.CreateInstance(type);
            return canvasWrapper;
        }

        private static void FormatAuthResponse(ResultContainer result, Utilities.Callback<ResultContainer> callback)
        {
            if (result.ResultDictionary == null)
            {
                callback(result);
                return;
            }

            IDictionary<string, object> authResponse;
            if (result.ResultDictionary.TryGetValue(CanvasFacebook.AuthResponseKey, out authResponse))
            {
                result.ResultDictionary.Remove(CanvasFacebook.AuthResponseKey);
                foreach (var item in authResponse)
                {
                    result.ResultDictionary[item.Key] = item.Value;
                }
            }

            // The JS SDK doesn't always store the permissions so request them before returning the results
            if (result.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey)
                && !result.ResultDictionary.ContainsKey(LoginResult.PermissionsKey))
            {
                var parameters = new Dictionary<string, string>()
                {
                    { "fields", "permissions" },
                    { Constants.AccessTokenKey, (string)result.ResultDictionary[LoginResult.AccessTokenKey] },
                };
                FacebookDelegate<IGraphResult> apiCallback = (IGraphResult r) =>
                {
                    IDictionary<string, object> permissionsJson;
                    if (r.ResultDictionary != null && r.ResultDictionary.TryGetValue("permissions", out permissionsJson))
                    {
                        IList<string> permissions = new List<string>();
                        IList<object> data;
                        if (permissionsJson.TryGetValue("data", out data))
                        {
                            foreach (var permission in data)
                            {
                                var permissionDictionary = permission as IDictionary<string, object>;
                                if (permissionDictionary != null)
                                {
                                    string status;
                                    if (permissionDictionary.TryGetValue("status", out status)
                                        && status.Equals("granted", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        string permissionName;
                                        if (permissionDictionary.TryGetValue("permission", out permissionName))
                                        {
                                            permissions.Add(permissionName);
                                        }
                                        else
                                        {
                                            FacebookLogger.Warn("Didn't find permission name");
                                        }
                                    }
                                    else
                                    {
                                        FacebookLogger.Warn("Didn't find status in permissions result");
                                    }
                                }
                                else
                                {
                                    FacebookLogger.Warn("Failed to case permission dictionary");
                                }
                            }
                        }
                        else
                        {
                            FacebookLogger.Warn("Failed to extract data from permissions");
                        }

                        result.ResultDictionary[LoginResult.PermissionsKey] = permissions.ToCommaSeparateList();
                    }
                    else
                    {
                        FacebookLogger.Warn("Failed to load permissions for access token");
                    }

                    callback(result);
                };
                FB.API(
                    "me",
                    HttpMethod.GET,
                    apiCallback,
                    parameters);
            }
            else
            {
                callback(result);
            }
        }

        private void PayImpl(
            string product,
            string productId,
            string action,
            int quantity,
            int? quantityMin,
            int? quantityMax,
            string requestId,
            string pricepointId,
            string testCurrency,
            string developerPayload,
            FacebookDelegate<IPayResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("product", product);
            args.AddString("product_id", productId);
            args.AddString("action", action);
            args.AddPrimative("quantity", quantity);
            args.AddNullablePrimitive("quantity_min", quantityMin);
            args.AddNullablePrimitive("quantity_max", quantityMax);
            args.AddString("request_id", requestId);
            args.AddString("pricepoint_id", pricepointId);
            args.AddString("test_currency", testCurrency);
            args.AddString("developer_payload", developerPayload);
            var call = new CanvasUIMethodCall<IPayResult>(this, MethodPay, Constants.OnPayCompleteMethodName);
            call.Callback = callback;
            call.Call(args);
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
                this.canvasImpl.wrapper.UI(clonedArgs.ToJsonString (), uniqueId, this.callbackMethod);
            }
        }
    }
}
