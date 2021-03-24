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

namespace Facebook.Unity.Tests.Mobile.IOS
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Facebook.Unity.Mobile;
    using Facebook.Unity.Mobile.IOS;

    internal class MockIOS : MockWrapper, IIOSWrapper
    {
        public void Init(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix)
        {
            this.LogMethodCall();

            // Handle testing of init returning access token. It would be nice
            // to not have init return the access token but this could be
            // a breaking change for people who read the raw result
            ResultContainer resultContainer;
            IDictionary<string, object> resultExtras = this.ResultExtras;
            if (resultExtras != null)
            {
                var result = MockResults.GetGenericResult(0, resultExtras);
                resultContainer = new ResultContainer(result);
            }
            else
            {
                resultContainer = new ResultContainer(string.Empty);
            }

            Facebook.OnInitComplete(resultContainer);
        }

        public void EnableProfileUpdatesOnAccessTokenChange(bool enable)
        {
            this.LogMethodCall();
        }

        public void LogInWithReadPermissions(
            int requestId,
            string scope)
        {
            this.LogMethodCall();
            this.LoginCommon(requestId, scope);
        }

        public void LogInWithPublishPermissions(
            int requestId,
            string scope)
        {
            this.LogMethodCall();
            this.LoginCommon(requestId, scope);
        }

        public void LoginWithTrackingPreference(
            int requestId,
            string scope,
            string tracking,
            string nonce)
        {
            this.LogMethodCall();
            this.LoginCommon(requestId, scope);
        }

        public void LogOut()
        {
            this.LogMethodCall();
        }

        public AuthenticationToken CurrentAuthenticationToken()
        {
            this.LogMethodCall();
            return null;
        }

        public Profile CurrentProfile()
        {
            this.LogMethodCall();
            return null;
        }

        public void SetPushNotificationsDeviceTokenString(string token)
        {
            this.LogMethodCall();
        }

        public void SetShareDialogMode(int mode)
        {
            this.LogMethodCall();
        }

        public void ShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL)
        {
            this.LogMethodCall();
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.Facebook.OnShareLinkComplete(new ResultContainer(result));
        }

        public void FeedShare(
            int requestId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string picture,
            string mediaSource)
        {
            this.LogMethodCall();
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.Facebook.OnShareLinkComplete(new ResultContainer(result));
        }

        public void AppRequest(
            int requestId,
            string message,
            string actionType,
            string objectId,
            string[] to = null,
            int toLength = 0,
            string filters = "",
            string[] excludeIds = null,
            int excludeIdsLength = 0,
            bool hasMaxRecipients = false,
            int maxRecipients = 0,
            string data = "",
            string title = "")
        {
            this.LogMethodCall();
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.Facebook.OnAppRequestsComplete(new ResultContainer(result));
        }

        public void FBAppEventsActivateApp()
        {
            this.LogMethodCall();
        }

        public void LogAppEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
            this.LogMethodCall();
        }

        public void LogPurchaseAppEvent(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
            this.LogMethodCall();
        }

        public void FBAppEventsSetLimitEventUsage(bool limitEventUsage)
        {
            this.LogMethodCall();
        }

        public void FBAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled)
        {
            this.LogMethodCall();
        }

        public bool FBAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            this.LogMethodCall();
            return true;
        }

        public void FBAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled)
        {
            this.LogMethodCall();
        }

        public void GetAppLink(int requestId)
        {
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.Facebook.OnGetAppLinkComplete(new ResultContainer(result));
        }

        public string FBSdkVersion()
        {
            return "1.0.0";
        }

        public void FetchDeferredAppLink(int requestId)
        {
            this.LogMethodCall();
        }

        public void OpenFriendFinderDialog(int requestId)
        {
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.MobileFacebook.OnFriendFinderComplete(new ResultContainer(result));
        }

        public void RefreshCurrentAccessToken(int requestID)
        {
            var result = MockResults.GetLoginResult(
                requestID,
                string.Empty,
                this.ResultExtras);
            this.MobileFacebook.OnRefreshCurrentAccessTokenComplete(new ResultContainer(result));
        }

        private void LoginCommon(
            int requestID,
            string scope)
        {
            var result = MockResults.GetLoginResult(
                requestID,
                scope,
            this.ResultExtras);
            this.Facebook.OnLoginComplete(new ResultContainer(result));
        }

        public void FBSetUserID(string userID)
        {
            this.LogMethodCall();
        }

        public string FBGetUserID()
        {
            this.LogMethodCall();
            return "1234";
        }

        public void UpdateUserProperties(int numParams, string[] paramKeys, string[] paramVals)
        {
            this.LogMethodCall();
        }

        public void SetDataProcessingOptions(string[] options, int country, int state)
        {
            this.LogMethodCall();
        }

        public void UploadImageToMediaLibrary(
            int requestId,
            string caption,
            string mediaUri,
            bool shouldLaunchMediaDialog)
        {
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.MobileFacebook.OnUploadImageToMediaLibraryComplete(new ResultContainer(result));
        }

        public void UploadVideoToMediaLibrary(
            int requestId,
            string caption,
            string mediaUri)
        {
            var result = MockResults.GetGenericResult(requestId, this.ResultExtras);
            this.MobileFacebook.OnUploadVideoToMediaLibraryComplete(new ResultContainer(result));
        }
    }
}
