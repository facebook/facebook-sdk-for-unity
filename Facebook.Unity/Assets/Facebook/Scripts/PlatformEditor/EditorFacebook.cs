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

namespace Facebook.Unity.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Editor.Dialogs;
    using Facebook.Unity.Mobile;
    using UnityEngine;

    internal class EditorFacebook : FacebookBase, IMobileFacebookImplementation, ICanvasFacebookImplementation
    {
        private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. " +
            "Behavior may not be the same as when used on iOS, Android, or Web.";

        private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";

        public EditorFacebook() : base(new CallbackManager())
        {
        }

        public override bool LimitEventUsage { get; set; }

        public ShareDialogMode ShareDialogMode { get; set; }

        public override string SDKName
        {
            get
            {
                return "FBUnityEditorSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return Facebook.Unity.FacebookSdkVersion.Build;
            }
        }

        private IFacebookCallbackHandler EditorGameObject
        {
            get
            {
                return ComponentFactory.GetComponent<EditorFacebookGameObject>();
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
            // Warn that editor behavior will not match supported platforms
            FacebookLogger.Warn(WarningMessage);

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

            this.EditorGameObject.OnInitComplete(string.Empty);
        }

        public override void LogInWithReadPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            // For the editor don't worry about the difference between
            // LogInWithReadPermissions and LogInWithPublishPermissions
            this.LogInWithPublishPermissions(permissions, callback);
        }

        public override void LogInWithPublishPermissions(
            IEnumerable<string> permissions,
            FacebookDelegate<ILoginResult> callback)
        {
            var dialog = ComponentFactory.GetComponent<MockLoginDialog>();
            dialog.Callback = this.EditorGameObject.OnLoginComplete;
            dialog.CallbackID = this.CallbackManager.AddFacebookDelegate(callback);
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
            this.ShowEmptyMockDialog(this.OnAppRequestsComplete, callback, "Mock App Request");
        }

        public override void ShareLink(
            Uri contentURL,
            string contentTitle,
            string contentDescription,
            Uri photoURL,
            FacebookDelegate<IShareResult> callback)
        {
            this.ShowEmptyMockDialog(this.OnShareLinkComplete, callback, "Mock Share Link");
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
            this.ShowEmptyMockDialog(this.OnShareLinkComplete, callback, "Mock Feed Share");
        }

        public override void GameGroupCreate(
            string name,
            string description,
            string privacy,
            FacebookDelegate<IGroupCreateResult> callback)
        {
            this.ShowEmptyMockDialog(this.OnGroupCreateComplete, callback, "Mock Group Create");
        }

        public override void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback)
        {
            this.ShowEmptyMockDialog(this.OnGroupJoinComplete, callback, "Mock Group Join");
        }

        public override void ActivateApp(string appId)
        {
            FacebookLogger.Info("This only needs to be called for iOS or Android.");
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            var result = new Dictionary<string, object>();
            result[Constants.UrlKey] = "mockurl://testing.url";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
            this.OnGetAppLinkComplete(MiniJSON.Json.Serialize(result));
        }

        public override void AppEventsLogEvent(
            string logEvent,
            float? valueToSum,
            Dictionary<string, object> parameters)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public override void AppEventsLogPurchase(
            float logPurchase,
            string currency,
            Dictionary<string, object> parameters)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public void AppInvite(
            Uri appLinkUrl,
            Uri previewImageUrl,
            FacebookDelegate<IAppInviteResult> callback)
        {
            this.ShowEmptyMockDialog(this.OnAppInviteComplete, callback, "Mock App Invite");
        }

        public void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            var result = new Dictionary<string, object>();
            result[Constants.UrlKey] = "mockurl://testing.url";
            result[Constants.RefKey] = "mock ref";
            result[Constants.ExtrasKey] = new Dictionary<string, object>()
            {
                {
                    "mock extra key", "mock extra value"
                }
            };

            result[Constants.TargetUrlKey] = "mocktargeturl://mocktarget.url";
            result[Constants.CallbackIdKey] = this.CallbackManager.AddFacebookDelegate(callback);
            this.OnFetchDeferredAppLinkComplete(MiniJSON.Json.Serialize(result));
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
            this.ShowEmptyMockDialog(this.OnPayComplete, callback, "Mock Pay Dialog");
        }

        public override void OnAppRequestsComplete(string message)
        {
            var result = new AppRequestResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGetAppLinkComplete(string message)
        {
            var result = new AppLinkResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupCreateComplete(string message)
        {
            var result = new GroupCreateResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupJoinComplete(string message)
        {
            var result = new GroupJoinResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnLoginComplete(string message)
        {
            var result = new LoginResult(message);
            this.OnAuthResponse(result);
        }

        public override void OnShareLinkComplete(string message)
        {
            var result = new ShareResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnAppInviteComplete(string message)
        {
            var result = new AppInviteResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnFetchDeferredAppLinkComplete(string message)
        {
            var result = new AppLinkResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnPayComplete(string message)
        {
            var result = new PayResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        #region Canvas Dummy Methods

        public void OnFacebookAuthResponseChange(string message)
        {
            throw new NotSupportedException();
        }

        public void OnUrlResponse(string message)
        {
            throw new NotSupportedException();
        }

        #endregion

        private void ShowEmptyMockDialog<T>(
            EditorFacebookMockDialog.OnComplete callback,
            FacebookDelegate<T> userCallback,
            string title) where T : IResult
        {
            var dialog = ComponentFactory.GetComponent<EmptyMockDialog>();
            dialog.Callback = callback;
            dialog.CallbackID = this.CallbackManager.AddFacebookDelegate(userCallback);
            dialog.EmptyDialogTitle = title;
        }
    }
}
