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
    using System.Linq;

    /// <summary>
    /// Classes defined on the mobile sdks
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

        public abstract void AppInvite(
            Uri appLinkUrl,
            Uri previewImageUrl,
            FacebookDelegate<IAppInviteResult> callback);

        public abstract void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback);

        public abstract void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback);

        public override void OnLoginComplete(string message)
        {
            var result = new LoginResult(message);
            this.OnAuthResponse(result);
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

        public override void OnAppRequestsComplete(string message)
        {
            var result = new AppRequestResult(message);
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

        public override void OnShareLinkComplete(string message)
        {
            var result = new ShareResult(message);
            CallbackManager.OnFacebookResponse(result);
        }

        public void OnRefreshCurrentAccessTokenComplete(string message)
        {
            var result = new AccessTokenRefreshResult(message);
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }

            CallbackManager.OnFacebookResponse(result);
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
