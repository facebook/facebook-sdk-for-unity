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

namespace Facebook.Unity.IOS
{
    using System.Runtime.InteropServices;
    using Facebook.Unity.Mobile.IOS;

    internal class IOSWrapper : IIOSWrapper
    {
        public void Init(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix)
        {
            IOSWrapper.IOSFBInit(
                appId,
                frictionlessRequests,
                urlSuffix,
                unityUserAgentSuffix);
        }

        public void LogInWithReadPermissions(
            int requestId,
            string scope)
        {
            IOSWrapper.IOSFBLogInWithReadPermissions(
                requestId,
                scope);
        }

        public void LogInWithPublishPermissions(
            int requestId,
            string scope)
        {
            IOSWrapper.IOSFBLogInWithPublishPermissions(
                requestId,
                scope);
        }

        public void LogOut()
        {
            IOSWrapper.IOSFBLogOut();
        }

        public void SetShareDialogMode(int mode)
        {
            IOSWrapper.IOSFBSetShareDialogMode(mode);
        }

        public void ShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL)
        {
            IOSWrapper.IOSFBShareLink(
                requestId,
                contentURL,
                contentTitle,
                contentDescription,
                photoURL);
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
            IOSWrapper.IOSFBFeedShare(
                requestId,
                toId,
                link,
                linkName,
                linkCaption,
                linkDescription,
                picture,
                mediaSource);
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
            IOSWrapper.IOSFBAppRequest(
                requestId,
                message,
                actionType,
                objectId,
                to,
                toLength,
                filters,
                excludeIds,
                excludeIdsLength,
                hasMaxRecipients,
                maxRecipients,
                data,
                title);
        }

        public void AppInvite(
            int requestId,
            string appLinkUrl,
            string previewImageUrl)
        {
            IOSWrapper.IOSFBAppInvite(
                requestId,
                appLinkUrl,
                previewImageUrl);
        }

        public void CreateGameGroup(
            int requestId,
            string name,
            string description,
            string privacy)
        {
            IOSWrapper.IOSFBCreateGameGroup(
                requestId,
                name,
                description,
                privacy);
        }

        public void JoinGameGroup(int requestId, string groupId)
        {
            IOSWrapper.IOSFBJoinGameGroup(requestId, groupId);
        }

        public void FBSettingsActivateApp(string appId)
        {
            IOSWrapper.IOSFBSettingsActivateApp(appId);
        }

        public void LogAppEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
            IOSWrapper.IOSFBAppEventsLogEvent(
                logEvent,
                valueToSum,
                numParams,
                paramKeys,
                paramVals);
        }

        public void LogPurchaseAppEvent(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
            IOSWrapper.IOSFBAppEventsLogPurchase(
                logPurchase,
                currency,
                numParams,
                paramKeys,
                paramVals);
        }

        public void FBAppEventsSetLimitEventUsage(bool limitEventUsage)
        {
            IOSWrapper.IOSFBAppEventsSetLimitEventUsage(limitEventUsage);
        }

        public void GetAppLink(int requestId)
        {
            IOSWrapper.IOSFBGetAppLink(requestId);
        }

        public string FBSdkVersion()
        {
            return IOSWrapper.IOSFBSdkVersion();
        }

        public void FetchDeferredAppLink(int requestId)
        {
            IOSWrapper.IOSFBFetchDeferredAppLink(requestId);
        }

        public void RefreshCurrentAccessToken(int requestId)
        {
            IOSWrapper.IOSFBRefreshCurrentAccessToken(requestId);
        }

        [DllImport("__Internal")]
        private static extern void IOSFBInit(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix);

        [DllImport("__Internal")]
        private static extern void IOSFBLogInWithReadPermissions(
            int requestId,
            string scope);

        [DllImport("__Internal")]
        private static extern void IOSFBLogInWithPublishPermissions(
            int requestId,
            string scope);

        [DllImport("__Internal")]
        private static extern void IOSFBLogOut();

        [DllImport("__Internal")]
        private static extern void IOSFBSetShareDialogMode(int mode);

        [DllImport("__Internal")]
        private static extern void IOSFBShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL);

        [DllImport("__Internal")]
        private static extern void IOSFBFeedShare(
            int requestId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string picture,
            string mediaSource);

        [DllImport("__Internal")]
        private static extern void IOSFBAppRequest(
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
            string title = "");

        [DllImport("__Internal")]
        private static extern void IOSFBAppInvite(
            int requestId,
            string appLinkUrl,
            string previewImageUrl);

        [DllImport("__Internal")]
        private static extern void IOSFBCreateGameGroup(
            int requestId,
            string name,
            string description,
            string privacy);

        [DllImport("__Internal")]
        private static extern void IOSFBJoinGameGroup(int requestId, string groupId);

        [DllImport("__Internal")]
        private static extern void IOSFBSettingsActivateApp(string appId);

        [DllImport("__Internal")]
        private static extern void IOSFBAppEventsLogEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        [DllImport("__Internal")]
        private static extern void IOSFBAppEventsLogPurchase(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        [DllImport("__Internal")]
        private static extern void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage);

        [DllImport("__Internal")]
        private static extern void IOSFBGetAppLink(int requestID);

        [DllImport("__Internal")]
        private static extern string IOSFBSdkVersion();

        [DllImport("__Internal")]
        private static extern void IOSFBFetchDeferredAppLink(int requestID);

        [DllImport("__Internal")]
        private static extern void IOSFBRefreshCurrentAccessToken(int requestID);
    }
}
