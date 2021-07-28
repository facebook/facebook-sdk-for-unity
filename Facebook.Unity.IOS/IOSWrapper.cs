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
    using System;
    using System.Collections.Generic;
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

        public void EnableProfileUpdatesOnAccessTokenChange(bool enable)
        {
            IOSWrapper.IOSFBEnableProfileUpdatesOnAccessTokenChange(enable);
        }

        public void LoginWithTrackingPreference(
            int requestId,
            string scope,
            string tracking,
            string nonce)
        {
            IOSWrapper.IOSFBLoginWithTrackingPreference(requestId, scope, tracking, nonce);
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

        public void SetPushNotificationsDeviceTokenString(string token)
        {
            IOSWrapper.IOSFBSetPushNotificationsDeviceTokenString(token);
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

        public void OpenFriendFinderDialog(
            int requestId)
        {
            IOSWrapper.IOSFBOpenGamingServicesFriendFinder(requestId);
        }

        public void FBAppEventsActivateApp()
        {
            IOSWrapper.IOSFBAppEventsActivateApp();
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

        public void FBAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled)
        {
           IOSWrapper.IOSFBAutoLogAppEventsEnabled(autoLogAppEventsEnabled);
        }

        public void FBAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled)
        {
            IOSWrapper.IOSFBAdvertiserIDCollectionEnabled(advertiserIDCollectionEnabled);
        }

        public bool FBAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            return IOSWrapper.IOSFBAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }

        public void GetAppLink(int requestId)
        {
            IOSWrapper.IOSFBGetAppLink(requestId);
        }

        public string FBSdkVersion()
        {
            return IOSWrapper.IOSFBSdkVersion();
        }

        public void FBSetUserID(string userID)
        {
            IOSWrapper.IOSFBSetUserID(userID);
        }

        public string FBGetUserID()
        {
            return IOSWrapper.IOSFBGetUserID();
        }

        public void UpdateUserProperties(
            int numParams,
            string[] paramKeys,
            string[] paramVals)
        {
            IOSWrapper.IOSFBUpdateUserProperties(numParams, paramKeys, paramVals);
        }

        public void SetDataProcessingOptions(string[] options, int country, int state)
        {
            IOSWrapper.IOSFBSetDataProcessingOptions(options, options.Length, country, state);
        }

        public AuthenticationToken CurrentAuthenticationToken()
        {
            String authenticationTokenString = IOSWrapper.IOSFBCurrentAuthenticationToken();
            if (String.IsNullOrEmpty(authenticationTokenString))
            {
                return null;
            }
            try
            {
                IDictionary<string, string> token = Utilities.ParseStringDictionaryFromString(authenticationTokenString);
                string tokenString;
                string nonce;
                token.TryGetValue("auth_token_string", out tokenString);
                token.TryGetValue("auth_nonce", out nonce);
                return new AuthenticationToken(tokenString, nonce);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Profile CurrentProfile()
        {
            String profileString = IOSWrapper.IOSFBCurrentProfile();
            if (String.IsNullOrEmpty(profileString))
            {
                return null;
            }
            try
            {
                IDictionary<string, string> profile = Utilities.ParseStringDictionaryFromString(profileString);
                string userID;
                string firstName;
                string middleName;
                string lastName;
                string name;
                string email;
                string imageURL;
                string linkURL;
                string friendIDs;
                string birthday;
                string gender;
                profile.TryGetValue("userID", out userID);
                profile.TryGetValue("firstName", out firstName);
                profile.TryGetValue("middleName", out middleName);
                profile.TryGetValue("lastName", out lastName);
                profile.TryGetValue("name", out name);
                profile.TryGetValue("email", out email);
                profile.TryGetValue("imageURL", out imageURL);
                profile.TryGetValue("linkURL", out linkURL);
                profile.TryGetValue("friendIDs", out friendIDs);
                profile.TryGetValue("birthday", out birthday);
                profile.TryGetValue("gender", out gender);

                UserAgeRange ageRange = UserAgeRange.AgeRangeFromDictionary(profile);
                FBLocation hometown = FBLocation.FromDictionary("hometown", profile);
                FBLocation location = FBLocation.FromDictionary("location", profile);
                return new Profile(
                    userID,
                    firstName,
                    middleName,
                    lastName,
                    name,
                    email,
                    imageURL,
                    linkURL,
                    friendIDs?.Split(','),
                    birthday,
                    ageRange,
                    hometown,
                    location,
                    gender);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UploadImageToMediaLibrary(
            int requestId,
            string caption,
            string imageUri,
            bool shouldLaunchMediaDialog)
        {
            IOSWrapper.IOSFBUploadImageToMediaLibrary(
                requestId,
                caption,
                imageUri,
                shouldLaunchMediaDialog);
        }

        public void UploadVideoToMediaLibrary(
            int requestId,
            string caption,
            string videoUri)
        {
            IOSWrapper.IOSFBUploadVideoToMediaLibrary(
                requestId,
                caption,
                videoUri);
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
        private static extern void IOSFBEnableProfileUpdatesOnAccessTokenChange(bool enable);

        [DllImport("__Internal")]
        private static extern void IOSFBLogInWithReadPermissions(
            int requestId,
            string scope);

        [DllImport("__Internal")]
        private static extern void IOSFBLoginWithTrackingPreference(
            int requestId,
            string scope,
            string tracking,
            string nonce);

        [DllImport("__Internal")]
        private static extern void IOSFBLogInWithPublishPermissions(
            int requestId,
            string scope);

        [DllImport("__Internal")]
        private static extern void IOSFBLogOut();

        [DllImport("__Internal")]
        private static extern void IOSFBSetPushNotificationsDeviceTokenString(string token);

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
        private static extern void IOSFBAppEventsActivateApp();

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
        private static extern void IOSFBAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled);

        [DllImport("__Internal")]
        private static extern void IOSFBAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabledID);

        [DllImport("__Internal")]
        private static extern bool IOSFBAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        [DllImport("__Internal")]
        private static extern void IOSFBGetAppLink(int requestID);

        [DllImport("__Internal")]
        private static extern string IOSFBSdkVersion();

        [DllImport("__Internal")]
        private static extern void IOSFBFetchDeferredAppLink(int requestID);

        [DllImport("__Internal")]
        private static extern void IOSFBRefreshCurrentAccessToken(int requestID);

        [DllImport("__Internal")]
        private static extern void IOSFBSetUserID(string userID);

        [DllImport("__Internal")]
        private static extern void IOSFBOpenGamingServicesFriendFinder(int requestID);

        [DllImport("__Internal")]
        private static extern void IOSFBUploadImageToMediaLibrary(
            int requestID,
            string caption,
            string imageUri,
            bool shouldLaunchMediaDialog);

        [DllImport("__Internal")]
        private static extern void IOSFBUploadVideoToMediaLibrary(
            int requestID,
            string caption,
            string videoUri);

        [DllImport("__Internal")]
        private static extern string IOSFBGetUserID();

        [DllImport("__Internal")]
        private static extern void IOSFBSetDataProcessingOptions(
            string[] options,
            int numOptions,
            int country,
            int state);

        [DllImport("__Internal")]
        private static extern void IOSFBUpdateUserProperties(
            int numParams,
            string[] paramKeys,
            string[] paramVals);

        [DllImport("__Internal")]
        private static extern string IOSFBCurrentAuthenticationToken();

        [DllImport("__Internal")]
        private static extern string IOSFBCurrentProfile();
    }
}
