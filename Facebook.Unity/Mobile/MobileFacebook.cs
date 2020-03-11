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

namespace Facebook.Unity.Mobile
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Classes defined on the mobile sdks.
    /// </summary>
    internal abstract class MobileFacebook : FacebookBase, IMobileFacebookImplementation
    {
        private const string CallbackIdKey = "callback_id";
        private ShareDialogMode shareDialogMode = ShareDialogMode.AUTOMATIC;

        protected MobileFacebook(CallbackManager callbackManager) : base(callbackManager)
        {
        }

        /// <summary>
        /// Gets or sets the dialog mode.
        /// </summary>
        /// <value>The dialog mode use for sharing, login, and other dialogs.</value>
        public ShareDialogMode ShareDialogMode
        {
            get
            {
                return this.shareDialogMode;
            }

            set
            {
                this.shareDialogMode = value;
                this.SetShareDialogMode(this.shareDialogMode);
            }
        }

        public abstract string UserID { get; set; }

        public abstract void UpdateUserProperties(Dictionary<string, string> parameters);

        public abstract void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback);

        public abstract void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback);

        public abstract bool IsImplicitPurchaseLoggingEnabled();

        public abstract void SetAutoLogAppEventsEnabled (bool autoLogAppEventsEnabled);

        public abstract void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled);

        public abstract void SetPushNotificationsDeviceTokenString(string token);

        public override void OnLoginComplete(ResultContainer resultContainer)
        {
            var result = new LoginResult(resultContainer);
            this.OnAuthResponse(result);
        }

        public override void OnGetAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnAppRequestsComplete(ResultContainer resultContainer)
        {
            var result = new AppRequestResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnFetchDeferredAppLinkComplete(ResultContainer resultContainer)
        {
            var result = new AppLinkResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(ResultContainer resultContainer)
        {
            var result = new ShareResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnRefreshCurrentAccessTokenComplete(ResultContainer resultContainer)
        {
            var result = new AccessTokenRefreshResult(resultContainer);
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }

            CallbackManager.OnFacebookResponse(result);
        }

        public virtual void OpenFriendFinderDialog(FacebookDelegate<IGamingServicesFriendFinderResult> callback)
        {
            throw new NotImplementedException();
        }

        public void OnFriendFinderComplete(ResultContainer resultContainer)
        {
            var result = new GamingServicesFriendFinderResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnUploadImageToMediaLibraryComplete(ResultContainer resultContainer)
        {
            var result = new MediaUploadResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnUploadVideoToMediaLibraryComplete(ResultContainer resultContainer)
        {
            var result = new MediaUploadResult(resultContainer);
            CallbackManager.OnFacebookResponse(result);
        }

        public virtual void UploadImageToMediaLibrary(
            string caption,
            Uri imageUri,
            bool shouldLaunchMediaDialog,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            throw new NotImplementedException();
        }

        public virtual void UploadVideoToMediaLibrary(
            string caption,
            Uri videoUri,
            FacebookDelegate<IMediaUploadResult> callback)
        {
            throw new NotImplementedException();
        }

        protected abstract void SetShareDialogMode(ShareDialogMode mode);

        private static IDictionary<string, object> DeserializeMessage(string message)
        {
            return (Dictionary<string, object>)MiniJSON.Json.Deserialize(message);
        }

        private static string SerializeDictionary(IDictionary<string, object> dict)
        {
            return MiniJSON.Json.Serialize(dict);
        }

        private static bool TryGetCallbackId(IDictionary<string, object> result, out string callbackId)
        {
            object callback;
            callbackId = null;
            if (result.TryGetValue("callback_id", out callback))
            {
                callbackId = callback as string;
                return true;
            }

            return false;
        }

        private static bool TryGetError(IDictionary<string, object> result, out string errorMessage)
        {
            object error;
            errorMessage = null;
            if (result.TryGetValue("error", out error))
            {
                errorMessage = error as string;
                return true;
            }

            return false;
        }
    }
}
