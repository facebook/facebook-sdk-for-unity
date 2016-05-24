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

namespace Facebook.Unity.Mobile.IOS
{
    using System.Runtime.InteropServices;

    internal class IOSWrapper : IIOSWrapper
    {
        public void Init(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix)
        {
            IOSWrapper.IOSInit(
                appId,
                frictionlessRequests,
                urlSuffix,
                unityUserAgentSuffix);
        }

        public void LogInWithReadPermissions(
            int requestId,
            string scope)
        {
            IOSWrapper.IOSLogInWithReadPermissions(
                requestId,
                scope);
        }

        public void LogInWithPublishPermissions(
            int requestId,
            string scope)
        {
            IOSWrapper.IOSLogInWithPublishPermissions(
                requestId,
                scope);
        }

        public void LogOut()
        {
            IOSWrapper.IOSLogOut();
        }

        public void SetShareDialogMode(int mode)
        {
            IOSWrapper.IOSSetShareDialogMode(mode);
        }

        public void ShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL)
        {
            IOSWrapper.IOSShareLink(
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
            IOSWrapper.IOSFeedShare(
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
            IOSWrapper.IOSAppRequest(
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
            IOSWrapper.IOSAppInvite(
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
            IOSWrapper.IOSCreateGameGroup(
                requestId,
                name,
                description,
                privacy);
        }

        public void JoinGameGroup(int requestId, string groupId)
        {
            IOSWrapper.IOSJoinGameGroup(requestId, groupId);
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
            IOSWrapper.IOSGetAppLink(requestId);
        }

        public string FBSdkVersion()
        {
            return IOSWrapper.IOSFBSdkVersion();
        }

        public void FetchDeferredAppLink(int requestId)
        {
            IOSWrapper.IOSFetchDeferredAppLink(requestId);
        }

        public void RefreshCurrentAccessToken(int requestId)
        {
            IOSWrapper.IOSRefreshCurrentAccessToken(requestId);
        }

        #if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern void IOSInit(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix);

        [DllImport ("__Internal")]
        private static extern void IOSLogInWithReadPermissions(
            int requestId,
            string scope);

        [DllImport ("__Internal")]
        private static extern void IOSLogInWithPublishPermissions(
            int requestId,
            string scope);

        [DllImport ("__Internal")]
        private static extern void IOSLogOut();

        [DllImport ("__Internal")]
        private static extern void IOSSetShareDialogMode(int mode);

        [DllImport ("__Internal")]
        private static extern void IOSShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL);

        [DllImport ("__Internal")]
        public static extern void IOSFeedShare(
            int requestId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string picture,
            string mediaSource);

        [DllImport ("__Internal")]
        private static extern void IOSAppRequest(
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

        [DllImport ("__Internal")]
        private static extern void IOSAppInvite(
            int requestId,
            string appLinkUrl,
            string previewImageUrl);

        [DllImport ("__Internal")]
        private static extern void IOSCreateGameGroup(
            int requestId,
            string name,
            string description,
            string privacy);

        [DllImport ("__Internal")]
        private static extern void IOSJoinGameGroup(int requestId, string groupId);

        [DllImport ("__Internal")]
        private static extern void IOSFBSettingsActivateApp(string appId);

        [DllImport ("__Internal")]
        private static extern void IOSFBAppEventsLogEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        [DllImport ("__Internal")]
        private static extern void IOSFBAppEventsLogPurchase(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        [DllImport ("__Internal")]
        private static extern void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage);

        [DllImport ("__Internal")]
        private static extern void IOSGetAppLink(int requestID);

        [DllImport ("__Internal")]
        private static extern string IOSFBSdkVersion();

        [DllImport ("__Internal")]
        private static extern void IOSFetchDeferredAppLink(int requestID);

        [DllImport ("__Internal")]
        private static extern void IOSRefreshCurrentAccessToken(int requestID);
        #else
        private static void IOSInit(
            string appId,
            bool frictionlessRequests,
            string urlSuffix,
            string unityUserAgentSuffix)
        {
        }

        private static void IOSLogInWithReadPermissions(
            int requestId,
            string scope)
        {
        }

        private static void IOSLogInWithPublishPermissions(
            int requestId,
            string scope)
        {
        }

        private static void IOSLogOut()
        {
        }

        private static void IOSSetShareDialogMode(int mode)
        {
        }

        private static void IOSShareLink(
            int requestId,
            string contentURL,
            string contentTitle,
            string contentDescription,
            string photoURL)
        {
        }

        private static void IOSFeedShare(
            int requestId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string picture,
            string mediaSource)
        {
        }

        private static void IOSAppRequest(
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
        }

        private static void IOSAppInvite(
            int requestId,
            string appLinkUrl,
            string previewImageUrl)
        {
        }

        private static void IOSCreateGameGroup(
            int requestId,
            string name,
            string description,
            string privacy)
        {
        }

        private static void IOSJoinGameGroup(int requestId, string groupId)
        {
        }

        private static void IOSFBSettingsActivateApp(string appId)
        {
        }

        private static void IOSFBAppEventsLogEvent(
            string logEvent,
            double valueToSum,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
        }

        private static void IOSFBAppEventsLogPurchase(
            double logPurchase,
            string currency,
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
        }

        private static void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage)
        {
        }

        private static void IOSGetAppLink(int requestId)
        {
        }

        private static string IOSFBSdkVersion()
        {
            return "NONE";
        }

        private static void IOSFetchDeferredAppLink(int requestId)
        {
        }

        private static void IOSRefreshCurrentAccessToken(int requestId)
        {
        }
        #endif
    }
}
