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

namespace Facebook.Unity.Gameroom
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal sealed class GameroomFacebook : FacebookBase,
    IGameroomFacebookImplementation
    {
        private string appId;
        private IGameroomWrapper gameroomWrapper;

        public GameroomFacebook() : this(GetGameroomWrapper(), new CallbackManager())
        {
        }

        public GameroomFacebook(IGameroomWrapper gameroomWrapper, CallbackManager callbackManager) : base(callbackManager)
        {
            this.gameroomWrapper = gameroomWrapper;
        }

        public delegate void OnComplete(ResultContainer resultContainer);

        public override bool LimitEventUsage { get; set; }

        public override string SDKName
        {
            get
            {
                return "FBGameroomSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return "0.0.1";
            }
        }

        public void Init(
            string appId,
            HideUnityDelegate hideUnityDelegate,
            InitDelegate onInitComplete)
        {
            base.Init(onInitComplete);
            this.appId = appId;
            this.gameroomWrapper.Init(this.OnInitComplete);
        }

        public override void ActivateApp(string appId = null)
        {
            this.AppEventsLogEvent(
                AppEventName.ActivatedApp,
                null,
                new Dictionary<string, object>());
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            var appEventParameters = parameters != null ? parameters : new Dictionary<string, object>();

            appEventParameters.Add("_eventName", logEvent);
            if (valueToSum.HasValue)
            {
                appEventParameters.Add("_valueToSum", valueToSum.Value);
            }

            var formData = new Dictionary<string, string>
            {
                { "event", "CUSTOM_APP_EVENTS" },
                { "application_tracking_enabled", "0" },
                { "advertiser_tracking_enabled", "0" },
                { "custom_events", string.Format("[{0}]", MiniJSON.Json.Serialize(appEventParameters)) },
            };

            FB.API(
                string.Format("{0}/activities", this.appId),
                HttpMethod.POST,
                null,
                formData);
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            parameters.Add("currency", currency);
            this.AppEventsLogEvent(
                AppEventName.Purchased,
                logPurchase,
                parameters);
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
            string filterString = null;
            if (filters != null)
            {
                using (var enumerator = filters.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        filterString = enumerator.Current as string;
                    }
                }
            }

            this.gameroomWrapper.DoAppRequestRequest(
              this.appId,
              message,
              actionType != null ? actionType.ToString() : null,
              objectId,
              to.ToCommaSeparateList(),
              filterString,
              excludeIds.ToCommaSeparateList(),
              maxRecipients.HasValue ? maxRecipients.Value.ToString() : null,
              data,
              title,
              this.CallbackManager.AddFacebookDelegate(callback),
              this.OnAppRequestsComplete);
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
            this.gameroomWrapper.DoFeedShareRequest(
                this.appId,
                toId,
                link != null ? link.ToString() : null,
                linkName,
                linkCaption,
                linkDescription,
                picture != null ? picture.ToString() : null,
                mediaSource,
                this.CallbackManager.AddFacebookDelegate(callback),
                this.OnShareLinkComplete);
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            this.FeedShare(
                null,
                contentURL,
                contentTitle,
                null,
                contentDescription,
                photoURL,
                null,
                callback);
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

        public void PayPremium(
            FacebookDelegate<IPayResult> callback)
        {
            this.gameroomWrapper.DoPayPremiumRequest(
                this.appId,
                this.CallbackManager.AddFacebookDelegate(callback),
                this.OnPayComplete);
        }

        public void HasLicense(
            FacebookDelegate<IHasLicenseResult> callback)
        {
            this.gameroomWrapper.DoHasLicenseRequest(
                this.appId,
                this.CallbackManager.AddFacebookDelegate(callback),
                this.OnHasLicenseComplete);
        }

        public override void GetAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            throw new NotSupportedException();
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> scope,
            FacebookDelegate<ILoginResult> callback)
        {
            this.LoginWithPermissions(scope, callback);
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> scope,
            FacebookDelegate<ILoginResult> callback)
        {
            this.LoginWithPermissions(scope, callback);
        }

        public override void OnAppRequestsComplete(ResultContainer resultContainer)
        {
            CallbackManager.OnFacebookResponse(new AppRequestResult(resultContainer));
        }

        public override void OnGetAppLinkComplete(ResultContainer resultContainer)
        {
            throw new NotSupportedException();
        }

        public override void OnLoginComplete(ResultContainer resultContainer)
        {
            this.OnAuthResponse(new LoginResult(resultContainer));
        }

        public override void OnShareLinkComplete(ResultContainer resultContainer)
        {
            CallbackManager.OnFacebookResponse(new ShareResult(resultContainer));
        }

        public void OnPayComplete(ResultContainer resultContainer)
        {
            CallbackManager.OnFacebookResponse(new PayResult(resultContainer));
        }

        public void OnHasLicenseComplete(ResultContainer resultContainer)
        {
            CallbackManager.OnFacebookResponse(new HasLicenseResult(resultContainer));
        }

        public bool HaveReceivedPipeResponse()
        {
            return this.gameroomWrapper.PipeResponse != null;
        }

        public string GetPipeResponse(string callbackId)
        {
            var response = this.gameroomWrapper.PipeResponse;
            this.gameroomWrapper.PipeResponse = null;
            response.Add(Constants.CallbackIdKey, callbackId);
            var jsonSerialization = Utilities.ToJson(response);
            return jsonSerialization;
        }

        private static IGameroomWrapper GetGameroomWrapper()
        {
            Assembly assembly = Assembly.Load("Facebook.Unity.Gameroom");
            Type type = assembly.GetType("Facebook.Unity.Gameroom.GameroomWrapper");
            IGameroomWrapper gameroomWrapper = (IGameroomWrapper)Activator.CreateInstance(type);
            return gameroomWrapper;
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
            this.gameroomWrapper.DoPayRequest(
                this.appId,
                "pay",
                action,
                product,
                productId,
                quantity.ToString(),
                quantityMin.HasValue ? quantityMin.Value.ToString() : null,
                quantityMax.HasValue ? quantityMax.Value.ToString() : null,
                requestId,
                pricepointId,
                testCurrency,
                developerPayload,
                this.CallbackManager.AddFacebookDelegate(callback),
                this.OnPayComplete);
        }

        private void LoginWithPermissions(
            IEnumerable<string> scope,
            FacebookDelegate<ILoginResult> callback)
        {
            this.gameroomWrapper.DoLoginRequest(
                this.appId,
                scope.ToCommaSeparateList(),
                this.CallbackManager.AddFacebookDelegate(callback),
                this.OnLoginComplete);
        }
    }
}
