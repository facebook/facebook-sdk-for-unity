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

namespace Facebook.Unity.Arcade
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal sealed class ArcadeFacebook : FacebookBase,
    IArcadeFacebookImplementation
    {
        private string appId;
        private IArcadeWrapper arcadeWrapper;

        public ArcadeFacebook() : this(GetArcadeWrapper(), new CallbackManager())
        {
        }

        public ArcadeFacebook(IArcadeWrapper arcadeWrapper, CallbackManager callbackManager) : base(callbackManager)
        {
            this.arcadeWrapper = arcadeWrapper;
        }

        public delegate void OnComplete(ResultContainer resultContainer);

        public override bool LimitEventUsage { get; set; }

        public override string SDKName
        {
            get
            {
                return "FBArcadeSDK";
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

            string accessTokenInfo;
            Utilities.CommandLineArguments.TryGetValue("/access_token", out accessTokenInfo);
            if (accessTokenInfo != null)
            {
                accessTokenInfo = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(accessTokenInfo));
                this.OnInitComplete(new ResultContainer(accessTokenInfo));
            }
            else
            {
                this.OnInitComplete(new ResultContainer(string.Empty));
            }
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

            this.arcadeWrapper.DoAppRequestRequest(
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
            this.arcadeWrapper.DoFeedShareRequest(
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

        public override void GameGroupCreate(
            string name,
            string description,
            string privacy,
            FacebookDelegate<IGroupCreateResult> callback)
        {
            throw new NotSupportedException();
        }

        public override void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback)
        {
            throw new NotSupportedException();
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

        public override void OnGroupCreateComplete(ResultContainer resultContainer)
        {
            throw new NotSupportedException();
        }

        public override void OnGroupJoinComplete(ResultContainer resultContainer)
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

        public bool HaveReceivedPipeResponse()
        {
            return this.arcadeWrapper.PipeResponse != null;
        }

        public string GetPipeResponse(string callbackId)
        {
            var response = this.arcadeWrapper.PipeResponse;
            this.arcadeWrapper.PipeResponse = null;
            response.Add(Constants.CallbackIdKey, callbackId);
            var jsonSerialization = Utilities.ToJson(response);
            return jsonSerialization;
        }

        private static IArcadeWrapper GetArcadeWrapper()
        {
            Assembly assembly = Assembly.Load("Facebook.Unity.Arcade");
            Type type = assembly.GetType("Facebook.Unity.Arcade.ArcadeWrapper");
            IArcadeWrapper arcadeWrapper = (IArcadeWrapper)Activator.CreateInstance(type);
            return arcadeWrapper;
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
            this.arcadeWrapper.DoPayRequest(
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
            this.arcadeWrapper.DoLoginRequest(
                this.appId,
                scope.ToCommaSeparateList(),
                this.CallbackManager.AddFacebookDelegate(callback),
                this.OnLoginComplete);
        }
    }
}
