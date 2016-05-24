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

namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal abstract class FacebookBase : IFacebookImplementation
    {
        private InitDelegate onInitCompleteDelegate;
        private HideUnityDelegate onHideUnityDelegate;

        protected FacebookBase(CallbackManager callbackManager)
        {
            this.CallbackManager = callbackManager;
        }

        public abstract bool LimitEventUsage { get; set; }

        public abstract string SDKName { get; }

        public abstract string SDKVersion { get; }

        public virtual string SDKUserAgent
        {
            get
            {
                return Utilities.GetUserAgent(this.SDKName, this.SDKVersion);
            }
        }

        public bool LoggedIn
        {
            get
            {
                AccessToken token = AccessToken.CurrentAccessToken;
                return token != null && token.ExpirationTime > DateTime.UtcNow;
            }
        }

        public bool Initialized { get; private set; }

        protected CallbackManager CallbackManager { get; private set; }

        public virtual void Init(
            HideUnityDelegate hideUnityDelegate,
            InitDelegate onInitComplete)
        {
            this.onHideUnityDelegate = hideUnityDelegate;
            this.onInitCompleteDelegate = onInitComplete;
        }

        public abstract void LogInWithPublishPermissions(
            IEnumerable<string> scope,
            FacebookDelegate<ILoginResult> callback);

        public abstract void LogInWithReadPermissions(
            IEnumerable<string> scope,
            FacebookDelegate<ILoginResult> callback);

        public virtual void LogOut()
        {
            AccessToken.CurrentAccessToken = null;
        }

        public void AppRequest(
            string message,
            IEnumerable<string> to = null,
            IEnumerable<object> filters = null,
            IEnumerable<string> excludeIds = null,
            int? maxRecipients = null,
            string data = "",
            string title = "",
            FacebookDelegate<IAppRequestResult> callback = null)
        {
            this.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
        }

        public abstract void AppRequest(
            string message,
            OGActionType? actionType,
            string objectId,
            IEnumerable<string> to,
            IEnumerable<object> filters,
            IEnumerable<string> excludeIds,
            int? maxRecipients,
            string data,
            string title,
            FacebookDelegate<IAppRequestResult> callback);

        public abstract void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback);

        public abstract void FeedShare(
            string toId,
            Uri link,
            string linkName,
            string linkCaption,
            string linkDescription,
            Uri picture,
            string mediaSource,
            FacebookDelegate<IShareResult> callback);

        public void API(
            string query,
            HttpMethod method,
            IDictionary<string, string> formData,
            FacebookDelegate<IGraphResult> callback)
        {
            IDictionary<string, string> inputFormData;

            // Copy the formData by value so it's not vulnerable to scene changes and object deletions
            inputFormData = (formData != null) ? this.CopyByValue(formData) : new Dictionary<string, string>();
            if (!inputFormData.ContainsKey(Constants.AccessTokenKey) && !query.Contains("access_token="))
            {
                inputFormData[Constants.AccessTokenKey] =
                    FB.IsLoggedIn ? AccessToken.CurrentAccessToken.TokenString : string.Empty;
            }

            AsyncRequestString.Request(this.GetGraphUrl(query), method, inputFormData, callback);
        }

        public void API(
            string query,
            HttpMethod method,
            WWWForm formData,
            FacebookDelegate<IGraphResult> callback)
        {
            if (formData == null)
            {
                formData = new WWWForm();
            }

            string tokenString = (AccessToken.CurrentAccessToken != null) ?
                AccessToken.CurrentAccessToken.TokenString : string.Empty;
            formData.AddField(
                Constants.AccessTokenKey,
                tokenString);

            AsyncRequestString.Request(this.GetGraphUrl(query), method, formData, callback);
        }

        public abstract void GameGroupCreate(
            string name,
            string description,
            string privacy,
            FacebookDelegate<IGroupCreateResult> callback);

        public abstract void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback);

        public abstract void ActivateApp(string appId = null);

        public abstract void GetAppLink(FacebookDelegate<IAppLinkResult> callback);

        public abstract void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters);

        public abstract void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters);

        public virtual void OnHideUnity(bool isGameShown)
        {
            if (this.onHideUnityDelegate != null)
            {
                this.onHideUnityDelegate(isGameShown);
            }
        }

        public void OnInitComplete(string message)
        {
            this.OnInitComplete(new ResultContainer(message));
        }

        public virtual void OnInitComplete(ResultContainer resultContainer)
        {
            this.Initialized = true;

            // Wait for the parsing of login to complete since we may need to pull
            // in more info about the access token returned
            FacebookDelegate<ILoginResult> loginCallback = (ILoginResult result) =>
            {
                if (this.onInitCompleteDelegate != null)
                {
                    this.onInitCompleteDelegate();
                }
            };

            resultContainer.ResultDictionary[Constants.CallbackIdKey]
                = this.CallbackManager.AddFacebookDelegate(loginCallback);
            this.OnLoginComplete(resultContainer);
        }

        public void OnLoginComplete(string message)
        {
            this.OnInitComplete(new ResultContainer(message));
        }

        public abstract void OnLoginComplete(ResultContainer resultContainer);

        public void OnLogoutComplete(ResultContainer resultContainer)
        {
            AccessToken.CurrentAccessToken = null;
        }

        public abstract void OnGetAppLinkComplete(ResultContainer resultContainer);

        public abstract void OnGroupCreateComplete(ResultContainer resultContainer);

        public abstract void OnGroupJoinComplete(ResultContainer resultContainer);

        public abstract void OnAppRequestsComplete(ResultContainer resultContainer);

        public abstract void OnShareLinkComplete(ResultContainer resultContainer);

        protected void ValidateAppRequestArgs(
            string message,
            OGActionType? actionType,
            string objectId,
            IEnumerable<string> to = null,
            IEnumerable<object> filters = null,
            IEnumerable<string> excludeIds = null,
            int? maxRecipients = null,
            string data = "",
            string title = "",
            FacebookDelegate<IAppRequestResult> callback = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message", "message cannot be null or empty!");
            }

            if (!string.IsNullOrEmpty(objectId) && !(actionType == OGActionType.ASKFOR || actionType == OGActionType.SEND))
            {
                throw new ArgumentNullException("objectId", "Object ID must be set if and only if action type is SEND or ASKFOR");
            }

            if (actionType == null && !string.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException("actionType", "You cannot provide an objectId without an actionType");
            }
        }

        protected void OnAuthResponse(LoginResult result)
        {
            // If the login is cancelled we won't have a access token.
            // Don't overwrite a valid token
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }

            this.CallbackManager.OnFacebookResponse(result);
        }

        private IDictionary<string, string> CopyByValue(IDictionary<string, string> data)
        {
            var newData = new Dictionary<string, string>(data.Count);
            foreach (KeyValuePair<string, string> kvp in data)
            {
                newData[kvp.Key] = kvp.Value != null ? new string(kvp.Value.ToCharArray()) : null;
            }

            return newData;
        }

        private Uri GetGraphUrl(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.StartsWith("/"))
            {
                query = query.Substring(1);
            }

            return new Uri(Constants.GraphUrl, query);
        }
    }
}
